//using Abp.Application.Services;
//using Abp.Application.Services.Dto;
//using Abp.Domain.Repositories;
//using Abp.Extensions;
//using Abp.Linq;
//using Abp.Linq.Extensions;
//using Abp.UI;
//using ELog.Application.CommonDto;
//using ELog.Application.Masters.UnitOfMeasurements.Dto;
//using ELog.Core;
//using ELog.Core.Authorization;
//using ELog.Core.Entities;
//using ELog.EntityFrameworkCore.EntityFrameworkCore.Repositories;
//using Microsoft.EntityFrameworkCore;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Linq.Dynamic.Core;
//using System.Threading.Tasks;
//using static ELog.Core.PMMSEnums;

//namespace ELog.Application.Masters.UnitOfMeasurements
//{
//    [PMMSAuthorize]
//    public class UnitOfMeasurementAppService : ApplicationService, IUnitOfMeasurementAppService
//    {
//        private readonly IRepository<UnitOfMeasurementMaster> _unitOfMeasurementRepository;
//        private readonly IRepository<UnitOfMeasurementTypeMaster> _unitOfMeasurementTypeRepository;
//        private readonly IRepository<WeighingMachineMaster> _weighingMachineRepository;
//        private readonly IMasterCommonRepository _masterCommonRepository;
//        private readonly IRepository<ApprovalStatusMaster> _approvalStatusRepository;
//        private readonly IRepository<StandardWeightMaster> _standardWeightRepository;
//        public IAsyncQueryableExecuter AsyncQueryableExecuter { get; set; }

//        public UnitOfMeasurementAppService(IRepository<UnitOfMeasurementMaster> unitOfMeasurementRepository,
//          IRepository<UnitOfMeasurementTypeMaster> unitOfMeasurementTypeRepository,
//          IRepository<WeighingMachineMaster> weighingMachineRepository,
//          IMasterCommonRepository masterCommonRepository,
//          IRepository<StandardWeightMaster> standardWeightRepository,
//          IRepository<ApprovalStatusMaster> approvalStatusRepository)

//        {
//            _unitOfMeasurementRepository = unitOfMeasurementRepository;
//            _unitOfMeasurementTypeRepository = unitOfMeasurementTypeRepository;
//            AsyncQueryableExecuter = NullAsyncQueryableExecuter.Instance;
//            _weighingMachineRepository = weighingMachineRepository;
//            _masterCommonRepository = masterCommonRepository;
//            _standardWeightRepository = standardWeightRepository;
//            _approvalStatusRepository = approvalStatusRepository;
//        }

//        [PMMSAuthorize(Permissions = PMMSPermissionConst.UnitOfMeasurement_SubModule + "." + PMMSPermissionConst.View)]
//        public async Task<UnitOfMeasurementDto> GetAsync(EntityDto<int> input)
//        {
//            var entity = await _unitOfMeasurementRepository.GetAsync(input.Id);
//            var unitOfMeasurement = ObjectMapper.Map<UnitOfMeasurementDto>(entity);
//            unitOfMeasurement.IsApprovalRequired = await _masterCommonRepository.IsApprovalRequired(PMMSPermissionConst.UnitOfMeasurement_SubModule);
//            unitOfMeasurement.UserEnteredApprovalStatus = ((ApprovalStatus)entity.ApprovalStatusId).ToString();
//            return unitOfMeasurement;
//        }

//        [PMMSAuthorize(Permissions = PMMSPermissionConst.UnitOfMeasurement_SubModule + "." + PMMSPermissionConst.View)]
//        public async Task<PagedResultDto<UnitOfMeasurementListDto>> GetAllAsync(PagedUnitOfMeasurementResultRequestDto input)
//        {
//            var query = CreateUserListFilteredQuery(input);

//            var totalCount = await AsyncQueryableExecuter.CountAsync(query);

//            query = ApplySorting(query, input);
//            query = ApplyPaging(query, input);

//            var entities = await AsyncQueryableExecuter.ToListAsync(query);

//            return new PagedResultDto<UnitOfMeasurementListDto>(
//                totalCount,
//                entities
//            );
//        }

//        [PMMSAuthorize(Permissions = PMMSPermissionConst.UnitOfMeasurement_SubModule + "." + PMMSPermissionConst.Add)]
//        public async Task<UnitOfMeasurementDto> CreateAsync(CreateUnitOfMeasurementDto input)
//        {
//            if (await IsUOMAndConversionUOMExist(input))
//            {
//                throw new UserFriendlyException(PMMSValidationConst.ValidationCode, PMMSValidationConst.UOMAlreadyExist);
//            }
//            var unitOfMeasurement = ObjectMapper.Map<UnitOfMeasurementMaster>(input);
//            unitOfMeasurement.ApprovalStatusId = (int)await _masterCommonRepository.GetApprovalForAdd(PMMSPermissionConst.UnitOfMeasurement_SubModule);
//            unitOfMeasurement.TenantId = AbpSession.TenantId;
//            var currentDate = DateTime.UtcNow;
//            unitOfMeasurement.UOMCode = $"U{currentDate.Month:D2}{currentDate:yy}{_masterCommonRepository.GetNextUOMSequence():D4}";
//            await _unitOfMeasurementRepository.InsertAsync(unitOfMeasurement);

//            CurrentUnitOfWork.SaveChanges();
//            return ObjectMapper.Map<UnitOfMeasurementDto>(unitOfMeasurement);
//        }

//        [PMMSAuthorize(Permissions = PMMSPermissionConst.UnitOfMeasurement_SubModule + "." + PMMSPermissionConst.Edit)]
//        public async Task<UnitOfMeasurementDto> UpdateAsync(UnitOfMeasurementDto input)
//        {
//            if (!input.IsActive)
//            {
//                var associatedEntities = await GetAllAssociatedMasters(input.Id);
//                if (associatedEntities.Count > 0)
//                {
//                    throw new UserFriendlyException(PMMSValidationConst.ValidationCode, PMMSValidationConst.UOMCannotDeactivated);
//                }
//            }
//            var unitOfMeasurement = await _unitOfMeasurementRepository.GetAsync(input.Id);
//            unitOfMeasurement.ApprovalStatusId = (int)await _masterCommonRepository.GetApprovalForEdit(PMMSPermissionConst.UnitOfMeasurement_SubModule, unitOfMeasurement.ApprovalStatusId);

//            ObjectMapper.Map(input, unitOfMeasurement);

//            await _unitOfMeasurementRepository.UpdateAsync(unitOfMeasurement);

//            return await GetAsync(input);
//        }

//        [PMMSAuthorize(Permissions = PMMSPermissionConst.UnitOfMeasurement_SubModule + "." + PMMSPermissionConst.Delete)]
//        public async Task DeleteAsync(EntityDto<int> input)
//        {
//            var associatedEntities = await GetAllAssociatedMasters(input.Id);
//            if (associatedEntities.Count > 0)
//            {
//                throw new UserFriendlyException(PMMSValidationConst.ValidationCode, PMMSValidationConst.UOMDelete);
//            }
//            var unitOfMeasurement = await _unitOfMeasurementRepository.GetAsync(input.Id).ConfigureAwait(false);
//            await _unitOfMeasurementRepository.DeleteAsync(unitOfMeasurement).ConfigureAwait(false);
//        }

//        [PMMSAuthorize(Permissions = PMMSPermissionConst.UnitOfMeasurement_SubModule + "." + PMMSPermissionConst.Approver)]
//        public async Task ApproveOrRejectUnitOfMeasurementAsync(ApprovalStatusDto input)
//        {
//            if (input.ApprovalStatusId == (int)ApprovalStatus.Rejected)
//            {
//                var associatedEntities = await GetAllAssociatedMasters(input.Id);
//                if (associatedEntities.Count > 0)
//                {
//                    throw new UserFriendlyException(PMMSValidationConst.ValidationCode, PMMSValidationConst.UOMRejected);
//                }
//            }
//            var equipment = await _unitOfMeasurementRepository.GetAsync(input.Id);
//            equipment.ApprovalStatusId = input.ApprovalStatusId;
//            equipment.ApprovalStatusDescription = input.Description;
//            await _unitOfMeasurementRepository.UpdateAsync(equipment);
//        }

//        private async Task<List<string>> GetAllAssociatedMasters(int id)
//        {
//            List<string> lstAssociatedEntities = new List<string>();
//            var entity = await (from unitOfMeasurement in _unitOfMeasurementRepository.GetAll()
//                                join weighingMachine in _weighingMachineRepository.GetAll()
//                               on unitOfMeasurement.Id equals weighingMachine.UnitOfMeasurementId into unitweighs
//                                from weighingMachine in unitweighs.DefaultIfEmpty()
//                                join standardWeight in _standardWeightRepository.GetAll()
//                                on unitOfMeasurement.Id equals standardWeight.UnitOfMeasurementId into standardUnit
//                                from standardWeight in standardUnit.DefaultIfEmpty()
//                                where unitOfMeasurement.Id == id
//                                select new
//                                {
//                                    weighingMachineUnitOfMeasurementId = weighingMachine.UnitOfMeasurementId,
//                                    standardWeightUnitOfMeasurement = standardWeight.UnitOfMeasurementId
//                                }).FirstOrDefaultAsync() ?? default;
//            if (entity?.weighingMachineUnitOfMeasurementId > 0)
//            {
//                lstAssociatedEntities.Add("Weighing Machine");
//            }
//            if (entity?.standardWeightUnitOfMeasurement > 0)
//            {
//                lstAssociatedEntities.Add("Standard Weight");
//            }

//            return lstAssociatedEntities;
//        }

//        private async Task<bool> IsUOMAndConversionUOMExist(CreateUnitOfMeasurementDto input)
//        {
//            var uom = input.UnitOfMeasurement.ToLower().Trim();
//            return await _unitOfMeasurementRepository.GetAll()
//                .AnyAsync(x => x.UnitOfMeasurement.ToLower() == uom);
//        }

//        /// <summary>
//        /// Should apply sorting if needed.
//        /// </summary>
//        /// <param name="query">The query.</param>
//        /// <param name="input">The input.</param>
//        protected IQueryable<UnitOfMeasurementListDto> ApplySorting(IQueryable<UnitOfMeasurementListDto> query, PagedUnitOfMeasurementResultRequestDto input)
//        {
//            //Try to sort query if available
//            ISortedResultRequest sortInput = input as ISortedResultRequest;
//            if (sortInput?.Sorting.IsNullOrWhiteSpace() == false)
//            {
//                return query.OrderBy(sortInput.Sorting);
//            }

//            //IQueryable.Task requires sorting, so we should sort if Take will be used.
//            if (input is ILimitedResultRequest)
//            {
//                return query.OrderByDescending(e => e.Id);
//            }

//            //No sorting
//            return query;
//        }

//        /// <summary>
//        /// Should apply paging if needed.
//        /// </summary>
//        /// <param name="query">The query.</param>
//        /// <param name="input">The input.</param>
//        protected IQueryable<UnitOfMeasurementListDto> ApplyPaging(IQueryable<UnitOfMeasurementListDto> query, PagedUnitOfMeasurementResultRequestDto input)
//        {
//            //Try to use paging if available
//            if (input is IPagedResultRequest pagedInput)
//            {
//                return query.PageBy(pagedInput);
//            }

//            //Try to limit query result if available
//            if (input is ILimitedResultRequest limitedInput)
//            {
//                return query.Take(limitedInput.MaxResultCount);
//            }

//            //No paging
//            return query;
//        }

//        protected IQueryable<UnitOfMeasurementListDto> CreateUserListFilteredQuery(PagedUnitOfMeasurementResultRequestDto input)
//        {
//            var query = from unitOFMeasurement in _unitOfMeasurementRepository.GetAll()
//                        join unitOfMeasurementType in _unitOfMeasurementTypeRepository.GetAll()
//                        on unitOFMeasurement.UnitOfMeasurementTypeId equals unitOfMeasurementType.Id into ut
//                        from unitOfMeasurementType in ut.DefaultIfEmpty()
//                        join approvalStatus in _approvalStatusRepository.GetAll()
//                        on unitOFMeasurement.ApprovalStatusId equals approvalStatus.Id into paStatus
//                        from approvalStatus in paStatus.DefaultIfEmpty()
//                        select new UnitOfMeasurementListDto
//                        {
//                            Id = unitOFMeasurement.Id,
//                            IsActive = unitOFMeasurement.IsActive,
//                            Name = unitOFMeasurement.Name,
//                            UnitOfMeasurementTypeId = unitOFMeasurement.UnitOfMeasurementTypeId,
//                            UOMCode = unitOFMeasurement.UOMCode,
//                            UnitOfMeasurement = unitOFMeasurement.UnitOfMeasurement,
//                            UserEnteredUOMType = unitOfMeasurementType.UnitOfMeasurementTypeName,
//                            ApprovalStatusId = unitOFMeasurement.ApprovalStatusId,
//                            UserEnteredApprovalStatus = approvalStatus.ApprovalStatus
//                        };
//            if (!(string.IsNullOrEmpty(input.Keyword) || string.IsNullOrWhiteSpace(input.Keyword)))
//            {
//                query = query.Where(x => x.UOMCode.Contains(input.Keyword) ||
//                                    x.Name.Contains(input.Keyword));
//            }
//            if (input.UnitOfMeasurementTypeId != null)
//            {
//                query = query.Where(x => x.UnitOfMeasurementTypeId == input.UnitOfMeasurementTypeId);
//            }
//            if (input.ActiveInactiveStatusId != null)
//            {
//                if (input.ActiveInactiveStatusId == (int)Status.In_Active)
//                {
//                    query = query.Where(x => !x.IsActive);
//                }
//                else if (input.ActiveInactiveStatusId == (int)Status.Active)
//                {
//                    query = query.Where(x => x.IsActive);
//                }
//            }
//            if (input.ApprovalStatusId != null)
//            {
//                query = query.Where(x => x.ApprovalStatusId == input.ApprovalStatusId);
//            }
//            return query;
//        }
//    }
//}