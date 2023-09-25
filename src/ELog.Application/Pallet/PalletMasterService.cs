using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq;
using Abp.Linq.Extensions;
using Abp.UI;
using ELog.Application.Masters.HandlingUnits.Dto;
using ELog.Application.Pallet.Dto;
using ELog.Core;
using ELog.Core.Authorization;
using ELog.Core.Entities;
using ELog.EntityFrameworkCore.EntityFrameworkCore.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using static ELog.Core.PMMSEnums;

namespace ELog.Application.Pallet
{
    [PMMSAuthorize]
    public class PalletMasterService : ApplicationService, IPalletMasterService
    {
        private readonly IRepository<PalletMaster> _palletRepository;
        private readonly IRepository<DepartmentMaster> _departmentRepository;
        private readonly IRepository<PlantMaster> _plantRepository;
        private readonly IRepository<StandardWeightMaster> _standardWeightRepository;
        private readonly IRepository<StandardWeightBoxMaster> _standardWeightBoxRepository;
        private readonly IRepository<LocationMaster> _locationRepository;
        private readonly IRepository<CubicleMaster> _cubicleRepository;
        private readonly IRepository<HandlingUnitMaster> _handlingUnitRepository;
        private readonly IRepository<HandlingUnitTypeMaster> _handlingUnitTypeMasterRepository;
        private const int approvedApprovalStatusId = (int)ApprovalStatus.Approved;
        private readonly IRepository<LabelPrintPacking> _labelPrintPackingRepository;
        private readonly IRepository<ProcessOrderAfterRelease> _processOrderRepository;
        private readonly IRepository<ProcessOrderMaterialAfterRelease> _processOrdermaterialRepository;



        public IAsyncQueryableExecuter AsyncQueryableExecuter { get; set; }
        private readonly IMasterCommonRepository _masterCommonRepository;

        public PalletMasterService(IRepository<PalletMaster> palletRepository, IRepository<DepartmentMaster> departmentRepository, IRepository<PlantMaster> plantRepository,
            IRepository<StandardWeightMaster> standardWeightRepository, IRepository<StandardWeightBoxMaster> standardWeightBoxRepository,
            IRepository<LocationMaster> locationRepository, IRepository<CubicleMaster> cubicleRepository,
            IMasterCommonRepository masterCommonRepository, IRepository<HandlingUnitMaster> handlingunitRepository, IRepository<HandlingUnitTypeMaster> _handlingUnitTypeMasterRepository,
            IRepository<LabelPrintPacking> labelPrintPackingRepository,
               IRepository<ProcessOrderAfterRelease> processOrderRepository,
                    IRepository<ProcessOrderMaterialAfterRelease> processOrdermaterialRepository)

        {

            _palletRepository = palletRepository;
            _plantRepository = plantRepository;
            _standardWeightRepository = standardWeightRepository;
            _standardWeightBoxRepository = standardWeightBoxRepository;
            _cubicleRepository = cubicleRepository;
            _locationRepository = locationRepository;
            _departmentRepository = departmentRepository;
            AsyncQueryableExecuter = NullAsyncQueryableExecuter.Instance;
            _masterCommonRepository = masterCommonRepository;
            _handlingUnitRepository = handlingunitRepository;
            _labelPrintPackingRepository = labelPrintPackingRepository;
            _processOrderRepository = processOrderRepository;
            _processOrdermaterialRepository = processOrdermaterialRepository;
        }

        [PMMSAuthorize(Permissions = "RecipeMaster.Add")]
        public async Task<PalletMasterDto> CreateAsync(PalletMasterDto input)
        {
            var checkValidUnit = await _handlingUnitRepository.GetAll().AnyAsync(x => x.HUCode == input.Pallet_Barcode && x.HandlingUnitTypeId == 3);
            if (!checkValidUnit)
            {
                throw new UserFriendlyException(PMMSValidationConst.ValidationCode, PMMSValidationConst.PalletNotThere);
            }

            var pallet = ObjectMapper.Map<PalletMaster>(input);
            pallet.TenantId = AbpSession.TenantId;
            var currentDate = DateTime.UtcNow;
            //pallet.AreaCode = $"A{currentDate.Month:D2}{currentDate:yy}{ _masterCommonRepository.GetNextUOMSequence():D4}";
            await _palletRepository.InsertAsync(pallet);

            CurrentUnitOfWork.SaveChanges();
            return ObjectMapper.Map<PalletMasterDto>(pallet);
        }

        [PMMSAuthorize(Permissions = "RecipeMaster.Delete")]
        public async Task DeleteAsync(EntityDto<int> input)
        {

            var pallet = await _palletRepository.GetAsync(input.Id).ConfigureAwait(false);
            await _palletRepository.DeleteAsync(pallet).ConfigureAwait(false);
        }

        [PMMSAuthorize(Permissions = "RecipeMaster.View")]
        public async Task<PalletMasterDto> GetAsync(EntityDto<int> input)
        {
            var entity = await _palletRepository.GetAsync(input.Id);
            return ObjectMapper.Map<PalletMasterDto>(entity);
        }

        public async Task<int> GetShipperCountAsync(string input)
        {
            int shipperCount = await _palletRepository.GetAll().Where(x => x.Pallet_Barcode == input).CountAsync();
            return shipperCount;

        }

        [PMMSAuthorize(Permissions = "RecipeMaster.View")]
        public async Task<PagedResultDto<PalletMasterListDto>> GetAllAsync(PagedPalletMasterResultRequestDto input)
        {
            var query = CreateUserListFilteredQuery(input);

            var totalCount = await AsyncQueryableExecuter.CountAsync(query);

            query = ApplySorting(query, input);
            query = ApplyPaging(query, input);

            var entities = await AsyncQueryableExecuter.ToListAsync(query);

            return new PagedResultDto<PalletMasterListDto>(
                totalCount,
                entities
            );
        }

        public async Task<PalletMasterDto> UpdateAsync(PalletMasterDto input)
        {
            var activity = await _palletRepository.GetAsync(input.Id);

            ObjectMapper.Map(input, activity);

            await _palletRepository.UpdateAsync(activity);

            return await GetAsync(input);
        }

        protected IQueryable<HandlingUnitListDto> CheckPalletQuery(PagedHandlingUnitResultRequestDto input)
        {
            var handlingUnitQuery = from handlingUnit in _handlingUnitRepository.GetAll()
                                    select new HandlingUnitListDto
                                    {
                                        Id = handlingUnit.Id,

                                    };

            if (input.PlantId != null)
            {
                handlingUnitQuery = handlingUnitQuery.Where(x => x.PlantId == input.PlantId);
            }



            return handlingUnitQuery;
        }

        protected IQueryable<PalletMasterListDto> ApplySorting(IQueryable<PalletMasterListDto> query, PagedPalletMasterResultRequestDto input)
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
        protected IQueryable<PalletMasterListDto> ApplyPaging(IQueryable<PalletMasterListDto> query, PagedPalletMasterResultRequestDto input)
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


        public async Task<List<PalletMasterDto>> GetGridShipperCodeAsync()
        {

            var PalletQuery = (from Pallet in _palletRepository.GetAll()
                               select new PalletMasterDto
                               {

                                   Carton_barcode = Pallet.Carton_barcode,


                               }).Distinct();


            return await PalletQuery.ToListAsync() ?? default;

        }
        protected IQueryable<PalletMasterListDto> CreateUserListFilteredQuery(PagedPalletMasterResultRequestDto input)
        {
            var PalletQuery = from Pallet in _palletRepository.GetAll()
                              select new PalletMasterListDto
                              {
                                  Id = Pallet.Id,
                                  Pallet_Barcode = Pallet.Pallet_Barcode,
                                  Carton_barcode = Pallet.Carton_barcode,
                                  Description = Pallet.Description,
                                  //ProductBatchNo = Pallet.ProductBatchNo,
                                  TenantId = Pallet.TenantId,
                                  PalletBarcodeId = Pallet.PalletBarcodeId,
                                  CartonBarcodeId = Pallet.CartonBarcodeId

                              };

            if (input.PalletCodeId != null)
            {
                PalletQuery = PalletQuery.Where(x => x.PalletBarcodeId == input.PalletCodeId);
            }
            if (input.CartonBarcodeId != null)
            {
                PalletQuery = PalletQuery.Where(x => x.CartonBarcodeId == input.CartonBarcodeId);
            }
            if (!(string.IsNullOrEmpty(input.Keyword) || string.IsNullOrWhiteSpace(input.Keyword)))
            {

                PalletQuery = PalletQuery.Where(x => x.Carton_barcode.Contains(input.Keyword) || x.Pallet_Barcode.Contains(input.Keyword) || x.Description.Contains(input.Keyword));

            }
            return PalletQuery;
        }

        public async Task<List<PalletMasterDto>> GetAllPalletBarcodeAsync(string input)
        {
            if (!(string.IsNullOrEmpty(input) || string.IsNullOrWhiteSpace(input)))
            {

                var palletBarcodeQuery = from handleUnit in _handlingUnitRepository.GetAll()
                                         where handleUnit.ApprovalStatusId == approvedApprovalStatusId && handleUnit.IsActive && handleUnit.HUCode.ToLower() == input.ToLower()
                                         select new PalletMasterDto
                                         {
                                             Id = handleUnit.Id,
                                             Pallet_Barcode = handleUnit.HUCode,

                                         };

                return await palletBarcodeQuery.ToListAsync() ?? default;
            }
            return default;
        }
        public async Task<List<PalletMasterDto>> GetAllShipperBarcodeAsync(string input)
        {
            if (!(string.IsNullOrEmpty(input) || string.IsNullOrWhiteSpace(input)))
            {

                var shipperQuery = from labelPrint in _labelPrintPackingRepository.GetAll()
                                   join material in _processOrdermaterialRepository.GetAll()
                                   on labelPrint.ProcessOrderId equals material.ProcessOrderId into pos
                                   from pro in pos.DefaultIfEmpty()
                                   where labelPrint.PackingLabelBarcode.ToLower() == input.ToLower()
                                   select new PalletMasterDto
                                   {
                                       Id = labelPrint.Id,
                                       Carton_barcode = labelPrint.PackingLabelBarcode,
                                       ContainerCount = labelPrint.ContainerCount,
                                       ProcessOrderId = labelPrint.ProcessOrderId.ToString(),
                                       ProductBatchNo = pro.ProductBatchNo
                                   };

                return await shipperQuery.ToListAsync() ?? default;
            }
            return default;
        }

    }
}
