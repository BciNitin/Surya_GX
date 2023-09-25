using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq;
using Abp.Linq.Extensions;
using Abp.UI;
using ELog.Application.CommonDto;
using ELog.Application.Inward.Palletizations.Dto;
using ELog.Core;
using ELog.Core.Authorization;
using ELog.Core.Entities;
using ELog.EntityFrameworkCore.EntityFrameworkCore.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using static ELog.Core.PMMSEnums;

namespace ELog.Application.Inward.Palletizations
{
    [PMMSAuthorize]
    public class PalletizationAppService : ApplicationService, IPalletizationAppService
    {
        #region fields

        public IAsyncQueryableExecuter AsyncQueryableExecuter { get; set; }
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IRepository<Palletization> _palletizationRepository;
        private readonly IRepository<HandlingUnitMaster> _handlingUnitRepository;
        private readonly IRepository<Material> _materialRepository;
        private readonly IMasterCommonRepository _masterCommonRepository;
        private const int approvedApprovalStatusId = (int)ApprovalStatus.Approved;

        #endregion fields

        #region constructor

        public PalletizationAppService(IRepository<Palletization> palletizationRepository,
            IHttpContextAccessor httpContextAccessor, IRepository<HandlingUnitMaster> handlingUnitRepository,
            IRepository<Material> materialRepository, IMasterCommonRepository masterCommonRepository
            )
        {
            AsyncQueryableExecuter = NullAsyncQueryableExecuter.Instance;
            _palletizationRepository = palletizationRepository;
            _handlingUnitRepository = handlingUnitRepository;
            _masterCommonRepository = masterCommonRepository;
            _materialRepository = materialRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        #endregion constructor

        #region public

        [PMMSAuthorize(Permissions = PMMSPermissionConst.Palletization_SubModule + "." + PMMSPermissionConst.View)]
        public async Task<List<PalletizationDto>> GetAsync(EntityDto<Guid> input)
        {
            var palletizationQuery = from palletization in _palletizationRepository.GetAll()
                                     join handlingUnit in _handlingUnitRepository.GetAll()
                                     on palletization.PalletId equals handlingUnit.Id
                                     join material in _materialRepository.GetAll()
                                     on palletization.MaterialId equals material.Id
                                     where palletization.TransactionId == input.Id && !palletization.IsUnloaded
                                     select new PalletizationDto
                                     {
                                         Id = palletization.Id,
                                         PalletBarcode = handlingUnit.HUCode + " - " + handlingUnit.Name,
                                         MaterialCode = material.ItemCode,
                                         MaterialDescription = material.ItemDescription,
                                         SAPBatchNumber = palletization.SAPBatchNumber,
                                         TransactionId = palletization.TransactionId,
                                         ContainerNo = palletization.ContainerNo,
                                         ContainerBarCode = palletization.ContainerBarCode,
                                         PalletId = handlingUnit.Id,
                                         MaterialId = material.Id,
                                     };
            return await palletizationQuery.ToListAsync() ?? default;
        }

        public async Task<PagedResultDto<PalletizationListDto>> GetAllAsync(PagedPalletizationResultRequestDto input)
        {
            var query = CreateUserListFilteredQuery(input);
            var totalCount = await AsyncQueryableExecuter.CountAsync(query);
            query = ApplySorting(query, input);
            query = ApplyPaging(query, input);
            var entities = await AsyncQueryableExecuter.ToListAsync(query);
            return new PagedResultDto<PalletizationListDto>(
                totalCount,
                entities
            );
        }

        [PMMSAuthorize(Permissions = PMMSPermissionConst.Palletization_SubModule + "." + PMMSPermissionConst.Add)]
        public async Task<PalletizationDto> CreateAsync(CreatePalletizationDto input)
        {
            var palletization = ObjectMapper.Map<Palletization>(input);
            if (input.TransactionId == null || input.TransactionId == Guid.Empty)
            {
                palletization.TransactionId = Guid.NewGuid();
            }
            IsPalletValid(input);
            palletization.TenantId = AbpSession.TenantId;
            await _palletizationRepository.InsertAsync(palletization);
            CurrentUnitOfWork.SaveChanges();
            var result = ObjectMapper.Map<PalletizationDto>(palletization);
            await MapPalletAndMaterial(input, result);
            return result;
        }

        [PMMSAuthorize(Permissions = PMMSPermissionConst.Palletization_SubModule + "." + PMMSPermissionConst.Edit)]
        public async Task UnloadPalletsAsync(Guid transactionId, List<PalletizationDto> PalletizationSelected)
        {
            var palletList = await _palletizationRepository.GetAllListAsync(x => x.TransactionId.Equals(transactionId)).ConfigureAwait(false);

            var newpalletlist = palletList.Where(y => PalletizationSelected.Any(z => z.ContainerBarCode == y.ContainerBarCode)).ToList();
            if (PalletizationSelected.Count == 0)
            {
                palletList.ForEach(x => x.IsUnloaded = true);
                await _masterCommonRepository.BulkUpdatePalletization(palletList);
            }
            else
            {
                newpalletlist.ForEach(x => x.IsUnloaded = true);
                await _masterCommonRepository.BulkUpdatePalletization(newpalletlist);
            }
        }

        public async Task<List<SelectListDtoWithPlantId>> GetAllPalletsAsync()
        {
            var plantId = _httpContextAccessor.HttpContext.Request.Headers["PlantId"].FirstOrDefault();
            var handlingUnitQuery = _handlingUnitRepository.GetAll().OrderBy(x => x.HUCode)
                      .Select(x => new SelectListDtoWithPlantId { Id = x.Id, Value = x.HUCode + " - " + x.Name, PlantId = x.PlantId });
            if (!(string.IsNullOrEmpty(plantId) || string.IsNullOrWhiteSpace(plantId)))
            {
                handlingUnitQuery = handlingUnitQuery.Where(x => x.PlantId == Convert.ToInt32(plantId));
            }
            return await handlingUnitQuery.Where(x => x.Value != null).ToListAsync() ?? default;
        }

        #endregion public

        #region private

        /// <summary>
        /// Should apply sorting if needed.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="input">The input.</param>
        protected IQueryable<PalletizationListDto> ApplySorting(IQueryable<PalletizationListDto> query, PagedPalletizationResultRequestDto input)
        {
            //Try to sort query if available
            var sortInput = input as ISortedResultRequest;
            if (sortInput?.Sorting.IsNullOrWhiteSpace() == false)
            {
                return query.OrderBy(sortInput.Sorting);
            }

            //IQueryable.Task requires sorting, so we should sort if Take will be used.
            if (input is ILimitedResultRequest)
            {
                return query.OrderByDescending(e => e.Id);
            }

            //No sorting
            return query;
        }

        /// <summary>
        /// Should apply paging if needed.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="input">The input.</param>
        protected IQueryable<PalletizationListDto> ApplyPaging(IQueryable<PalletizationListDto> query, PagedPalletizationResultRequestDto input)
        {
            //Try to use paging if available
            if (input is IPagedResultRequest pagedInput)
            {
                return query.PageBy(pagedInput);
            }

            //Try to limit query result if available
            if (input is ILimitedResultRequest limitedInput)
            {
                return query.Take(limitedInput.MaxResultCount);
            }

            //No paging
            return query;
        }

        public async Task<List<SelectListDtoWithPlantId>> GetAllPalletsBarcodeAsync(string input)
        {
            var plantId = _httpContextAccessor.HttpContext.Request.Headers["PlantId"].FirstOrDefault();
            var handlingUnitQuery = (from handlingUnitMaster in _handlingUnitRepository.GetAll()
                                     where handlingUnitMaster.HUCode.ToLower() == input.ToLower()
                                     && handlingUnitMaster.ApprovalStatusId == approvedApprovalStatusId && handlingUnitMaster.IsActive
                                     orderby handlingUnitMaster.HUCode
                                     select new SelectListDtoWithPlantId
                                     {
                                         Id = handlingUnitMaster.Id,
                                         Value = string.IsNullOrEmpty(handlingUnitMaster.Name) ? handlingUnitMaster.HUCode : $"{handlingUnitMaster.HUCode} - {handlingUnitMaster.Name}",
                                         PlantId = handlingUnitMaster.PlantId
                                     });

            if (!(string.IsNullOrEmpty(plantId) || string.IsNullOrWhiteSpace(plantId)))
            {
                handlingUnitQuery = handlingUnitQuery.Where(x => x.PlantId == Convert.ToInt32(plantId));
            }
            return await handlingUnitQuery.Distinct().ToListAsync() ?? default;
        }

        protected IQueryable<PalletizationListDto> CreateUserListFilteredQuery(PagedPalletizationResultRequestDto input)
        {
            var plantId = _httpContextAccessor.HttpContext.Request.Headers["PlantId"].FirstOrDefault();
            var palletizationQuery = from palletization in _palletizationRepository.GetAll()
                                     join handlingUnit in _handlingUnitRepository.GetAll()
                                     on palletization.PalletId equals handlingUnit.Id
                                     join material in _materialRepository.GetAll()
                                     on palletization.MaterialId equals material.Id
                                     where !palletization.IsUnloaded
                                     group new { palletization } by new
                                     {
                                         Id = palletization.TransactionId.ToString(),
                                         PalletBarcode = handlingUnit.HUCode + " - " + handlingUnit.Name,
                                         PalletId = palletization.PalletId,
                                         MaterialId = palletization.MaterialId,
                                         GRNDetailId = palletization.GRNDetailId,
                                         TransactionId = palletization.TransactionId,
                                         SAPBatchNumber = palletization.SAPBatchNumber,
                                         MaterialCode = material.ItemCode,
                                         plantId = handlingUnit.PlantId,
                                         MaterialDescription = material.ItemDescription,

                                     } into gcs
                                     select new PalletizationListDto
                                     {
                                         Id = gcs.Key.Id,
                                         PalletBarcode = gcs.Key.PalletBarcode,
                                         MaterialCode = gcs.Key.MaterialCode,
                                         SAPBatchNumber = gcs.Key.SAPBatchNumber,
                                         TransactionId = gcs.Key.TransactionId,
                                         PalletId = gcs.Key.PalletId,
                                         MaterialId = gcs.Key.MaterialId,
                                         PlantId = gcs.Key.plantId,
                                         MaterialDescription = gcs.Key.MaterialDescription,
                                         Count = gcs.Count(),
                                     };
            if (!(string.IsNullOrEmpty(plantId) || string.IsNullOrWhiteSpace(plantId)))
            {
                palletizationQuery = palletizationQuery.Where(x => x.PlantId == Convert.ToInt32(plantId));
            }
            if (input.PalletId != null)
            {
                palletizationQuery = palletizationQuery.Where(x => x.PalletId == input.PalletId);
            }
            if (input.MaterialId != null)
            {
                palletizationQuery = palletizationQuery.Where(x => x.MaterialId == input.MaterialId);
            }
            return palletizationQuery;
        }

        private void IsPalletValid(CreatePalletizationDto palletizationDto)
        {
            var materials = (from palletization in _palletizationRepository.GetAll()
                             where palletization.PalletId == palletizationDto.PalletId && !palletization.IsUnloaded
                             select new PalletizationDto
                             {
                                 MaterialId = palletization.MaterialId.HasValue ? palletization.MaterialId.Value : 0,
                                 SAPBatchNumber = palletization.SAPBatchNumber,
                                 ContainerId = palletization.Id
                             }).Distinct().ToList();

            var partitions = materials.GroupBy(p => new { p.MaterialId, p.SAPBatchNumber });
            if (partitions.Count() >= 6)
            {
                throw new UserFriendlyException(PMMSValidationConst.ValidationCode, string.Format(PMMSValidationConst.PartitionNotValid));
            }
            else
            {
                var isBarcodePresent = _palletizationRepository.GetAll().Where(a => a.MaterialId == palletizationDto.MaterialId && a.ContainerId == palletizationDto.ContainerId && !a.IsUnloaded);
                if (isBarcodePresent.Any())
                {
                    throw new UserFriendlyException(PMMSValidationConst.ValidationCode, string.Format(PMMSValidationConst.DuplicateMaterialBarcode));
                }
            }
        }

        private async Task MapPalletAndMaterial(CreatePalletizationDto input, PalletizationDto result)
        {
            result.PalletBarcode = input.PalletBarcode;
            var materialDetails = await _materialRepository.FirstOrDefaultAsync(x => x.Id == input.MaterialId);
            result.MaterialDescription = materialDetails.ItemDescription;
            result.MaterialCode = materialDetails.ItemCode;
        }

        #endregion private
    }
}