using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq;
using Abp.Linq.Extensions;
using Abp.UI;
using ELog.Application.CommonDto;
using ELog.Application.Masters.WeighingMachines.Dto;
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

namespace ELog.Application.Masters.WeighingMachines
{
    [PMMSAuthorize]
    public class WeighingMachineAppService : ApplicationService, IWeighingMachineAppService
    {
        private readonly IRepository<WeighingMachineMaster> _weighingMachineRepository;
        private readonly IRepository<PlantMaster> _plantMasterRepository;
        private readonly IRepository<UnitOfMeasurementMaster> _unitOfMeasurementRepository;
        private readonly IRepository<CalibrationFrequencyMaster> _calibrationFrequencyRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMasterCommonRepository _masterCommonRepository;
        private readonly IRepository<ApprovalStatusMaster> _approvalStatusRepository;
        private readonly IRepository<WeighingMachineTestConfiguration> _testConfigurationRepository;
        public IAsyncQueryableExecuter AsyncQueryableExecuter { get; set; }

        public WeighingMachineAppService(IRepository<WeighingMachineMaster> weighingMachineRepository,
            IRepository<PlantMaster> plantMasterRepository,
            IRepository<UnitOfMeasurementMaster> unitOfMeasurementRepository,
             IRepository<CalibrationFrequencyMaster> calibrationFrequencyRepository,
             IMasterCommonRepository masterCommonRepository,
             IRepository<ApprovalStatusMaster> approvalStatusRepository,
             IRepository<WeighingMachineTestConfiguration> testConfigurationRepository,
             IHttpContextAccessor httpContextAccessor
            )

        {
            _weighingMachineRepository = weighingMachineRepository;
            _plantMasterRepository = plantMasterRepository;
            _unitOfMeasurementRepository = unitOfMeasurementRepository;
            _calibrationFrequencyRepository = calibrationFrequencyRepository;
            _masterCommonRepository = masterCommonRepository;
            _approvalStatusRepository = approvalStatusRepository;
            _testConfigurationRepository = testConfigurationRepository;
            _httpContextAccessor = httpContextAccessor;
            AsyncQueryableExecuter = NullAsyncQueryableExecuter.Instance;
        }

        [PMMSAuthorize(Permissions = PMMSPermissionConst.WeighingMachine_SubModule + "." + PMMSPermissionConst.View)]
        public async Task<WeighingMachineDto> GetAsync(EntityDto<int> input)
        {
            var entity = await _weighingMachineRepository.GetAllIncluding(x => x.Calibrations, x => x.WeighingMachineTestConfigurations)
                .Where(x => x.Id == input.Id).AsNoTracking().FirstOrDefaultAsync();
            if (entity != null)
            {
                var weighingMachine = ObjectMapper.Map<WeighingMachineDto>(entity);
                var weighingCalibrations = entity?.Calibrations;
                weighingMachine.IsApprovalRequired = await _masterCommonRepository.IsApprovalRequired(PMMSPermissionConst.WeighingMachine_SubModule);
                weighingMachine.UserEnteredApprovalStatus = ((ApprovalStatus)entity.ApprovalStatusId).ToString();
                if (weighingCalibrations?.Count > 0)
                {
                    weighingMachine.Calibrations = weighingCalibrations.Select(MapCalibrationMaster).ToList();
                }
                return weighingMachine;
            }

            return default;
        }

        [PMMSAuthorize(Permissions = PMMSPermissionConst.WeighingMachine_SubModule + "." + PMMSPermissionConst.View)]
        public async Task<PagedResultDto<WeighingMachineListDto>> GetAllAsync(PagedWeighingMachineResultRequestDto input)
        {
            var query = CreateUserListFilteredQuery(input);

            var totalCount = await AsyncQueryableExecuter.CountAsync(query);

            query = ApplySorting(query, input);
            query = ApplyPaging(query, input);

            var entities = await AsyncQueryableExecuter.ToListAsync(query);

            return new PagedResultDto<WeighingMachineListDto>(
                totalCount,
                entities
            );
        }

        [PMMSAuthorize(Permissions = PMMSPermissionConst.WeighingMachine_SubModule + "." + PMMSPermissionConst.Add)]
        public async Task<WeighingMachineDto> CreateAsync(CreateWeighingMachineDto input)
        {
            input.WeighingMachineCode = input.WeighingMachineCode.Trim();
            if (_weighingMachineRepository.GetAll().Where(x => x.SubPlantId == input.SubPlantId && x.WeighingMachineCode == input.WeighingMachineCode).Any())
            {
                throw new UserFriendlyException(PMMSValidationConst.ValidationCode, PMMSValidationConst.WeighingMachineAlreadyExist);
            }
            var weighingMachine = ObjectMapper.Map<WeighingMachineMaster>(input);
            weighingMachine.ApprovalStatusId = (int)await _masterCommonRepository.GetApprovalForAdd(PMMSPermissionConst.WeighingMachine_SubModule);
            weighingMachine.TenantId = AbpSession.TenantId;
            weighingMachine.Calibrations = new List<CalibrationFrequencyMaster>();
            foreach (var calibration in input.Calibrations)
            {
                var calibrationToInsert = ObjectMapper.Map<CalibrationFrequencyMaster>(calibration);
                calibrationToInsert.TenantId = AbpSession.TenantId;
                weighingMachine.Calibrations.Add(calibrationToInsert);
            }
            weighingMachine.WeighingMachineTestConfigurations = new List<WeighingMachineTestConfiguration>();
            foreach (var configuration in input.WeighingMachineTestConfigurations)
            {
                var testConfiguration = ObjectMapper.Map<WeighingMachineTestConfiguration>(configuration);
                weighingMachine.WeighingMachineTestConfigurations.Add(testConfiguration);
            }

            await _weighingMachineRepository.InsertAsync(weighingMachine);

            return ObjectMapper.Map<WeighingMachineDto>(weighingMachine);
        }

        [PMMSAuthorize(Permissions = PMMSPermissionConst.WeighingMachine_SubModule + "." + PMMSPermissionConst.Edit)]
        public async Task<WeighingMachineDto> UpdateAsync(WeighingMachineDto input)
        {
            input.WeighingMachineCode = input.WeighingMachineCode.Trim();
            if (_weighingMachineRepository.GetAll().Where(x => x.Id != input.Id &&
                                                          x.SubPlantId == input.SubPlantId &&
                                                          x.WeighingMachineCode == input.WeighingMachineCode).Any())
            {
                throw new UserFriendlyException(PMMSValidationConst.ValidationCode, PMMSValidationConst.WeighingMachineAlreadyExist);
            }
            var weighingMachine = await _weighingMachineRepository.GetAsync(input.Id);
            weighingMachine.ApprovalStatusId = (int)await _masterCommonRepository.GetApprovalForEdit(PMMSPermissionConst.WeighingMachine_SubModule, weighingMachine.ApprovalStatusId);
            ObjectMapper.Map(input, weighingMachine);
            weighingMachine.WeighingMachineTestConfigurations = new List<WeighingMachineTestConfiguration>();
            await _weighingMachineRepository.UpdateAsync(weighingMachine);

            //Insert Calibration
            foreach (var calibration in input.Calibrations.Where(x => x.Id == 0))
            {
                var calibrationToInsert = ObjectMapper.Map<CalibrationFrequencyMaster>(calibration);
                calibrationToInsert.WeighingMachineId = input.Id;
                calibrationToInsert.TenantId = AbpSession.TenantId;
                await _calibrationFrequencyRepository.InsertAsync(calibrationToInsert);
            }

            var weighingMachineCalibrations = await _calibrationFrequencyRepository.GetAll().Where(x => x.WeighingMachineId == input.Id).ToListAsync();
            var inputCalibrationsToUpdateIds = input.Calibrations.Where(x => x.Id > 0).Select(x => x.Id);

            //Update Calibrations
            foreach (var calibrationId in inputCalibrationsToUpdateIds)
            {
                var calibrationFromUpdate = input.Calibrations.First(x => x.Id == calibrationId);
                var calibrationToUpdate = weighingMachineCalibrations.FirstOrDefault(x => x.Id == calibrationId);
                if (calibrationToUpdate != null)
                {
                    ObjectMapper.Map(calibrationFromUpdate, calibrationToUpdate);
                    await _calibrationFrequencyRepository.UpdateAsync(calibrationToUpdate);
                }
            }

            //Delete Calibrations
            foreach (var calibrationToDelete in weighingMachineCalibrations.Where(x => !inputCalibrationsToUpdateIds.Contains(x.Id)))
            {
                await _calibrationFrequencyRepository.DeleteAsync(calibrationToDelete);
            }

            //Insert Test Configuration
            foreach (var testConfiguration in input.WeighingMachineTestConfigurations.Where(x => x.Id == 0))
            {
                var testConfigurationToInsert = ObjectMapper.Map<WeighingMachineTestConfiguration>(testConfiguration);
                testConfiguration.WeighingMachineId = input.Id;
                await _testConfigurationRepository.InsertAsync(testConfigurationToInsert);
            }

            var weighingMachineTestConfigurations = await _testConfigurationRepository.GetAll().Where(x => x.WeighingMachineId == input.Id).ToListAsync();
            var inputTestConfigurationToUpdateIds = input.WeighingMachineTestConfigurations.Where(x => x.Id > 0).Select(x => x.Id);

            //Update Test Configuration
            foreach (var testConfigurationId in inputTestConfigurationToUpdateIds)
            {
                var testConfigurationFromUpdate = input.WeighingMachineTestConfigurations.First(x => x.Id == testConfigurationId);
                var testConfigurationToUpdate = weighingMachineTestConfigurations.FirstOrDefault(x => x.Id == testConfigurationId);
                if (testConfigurationToUpdate != null)
                {
                    ObjectMapper.Map(testConfigurationFromUpdate, testConfigurationToUpdate);
                    await _testConfigurationRepository.UpdateAsync(testConfigurationToUpdate);
                }
            }

            //Delete Calibrations
            foreach (var testConfigurationToDelete in weighingMachineTestConfigurations.Where(x => !inputTestConfigurationToUpdateIds.Contains(x.Id)))
            {
                await _testConfigurationRepository.DeleteAsync(testConfigurationToDelete);
            }

            return await GetAsync(input);
        }

        [PMMSAuthorize(Permissions = PMMSPermissionConst.WeighingMachine_SubModule + "." + PMMSPermissionConst.Delete)]
        public async Task DeleteAsync(EntityDto<int> input)
        {
            var weighingMachine = await _weighingMachineRepository.GetAsync(input.Id).ConfigureAwait(false);
            await _weighingMachineRepository.DeleteAsync(weighingMachine).ConfigureAwait(false);
        }

        [PMMSAuthorize(Permissions = PMMSPermissionConst.WeighingMachine_SubModule + "." + PMMSPermissionConst.Approver)]
        public async Task ApproveOrRejectWeighingMachineAsync(ApprovalStatusDto input)
        {
            var weighingMachine = await _weighingMachineRepository.GetAsync(input.Id);

            weighingMachine.ApprovalStatusId = input.ApprovalStatusId;
            weighingMachine.ApprovalStatusDescription = input.Description;
            await _weighingMachineRepository.UpdateAsync(weighingMachine);
        }

        private CalibrationFrequencyDto MapCalibrationMaster(CalibrationFrequencyMaster input)
        {
            return ObjectMapper.Map<CalibrationFrequencyDto>(input);
        }

        /// <summary>
        /// Should apply sorting if needed.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="input">The input.</param>
        protected IQueryable<WeighingMachineListDto> ApplySorting(IQueryable<WeighingMachineListDto> query, PagedWeighingMachineResultRequestDto input)
        {
            //Try to sort query if available
            var sortInput = input as ISortedResultRequest;
            if (sortInput != null && !sortInput.Sorting.IsNullOrWhiteSpace())
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
        protected IQueryable<WeighingMachineListDto> ApplyPaging(IQueryable<WeighingMachineListDto> query, PagedWeighingMachineResultRequestDto input)
        {
            //Try to use paging if available
            var pagedInput = input as IPagedResultRequest;
            if (pagedInput != null)
            {
                return query.PageBy(pagedInput);
            }

            //Try to limit query result if available
            var limitedInput = input as ILimitedResultRequest;
            if (limitedInput != null)
            {
                return query.Take(limitedInput.MaxResultCount);
            }

            //No paging
            return query;
        }

        protected IQueryable<WeighingMachineListDto> CreateUserListFilteredQuery(PagedWeighingMachineResultRequestDto input)
        {
            var subPlantIdHeader = _httpContextAccessor.HttpContext.Request.Headers["PlantId"].FirstOrDefault();
            var query = from weighingMachine in _weighingMachineRepository.GetAll()
                        join plantMaster in _plantMasterRepository.GetAll()
                        on weighingMachine.SubPlantId equals plantMaster.Id into weighMachinePlants
                        from plantMaster in weighMachinePlants.DefaultIfEmpty()
                        join unitOfMeasurement in _unitOfMeasurementRepository.GetAll()
                        on weighingMachine.UnitOfMeasurementId equals unitOfMeasurement.Id into weighMachineMeasurement
                        from unitOfMeasurement in weighMachineMeasurement.DefaultIfEmpty()
                        join approvalStatus in _approvalStatusRepository.GetAll()
                             on weighingMachine.ApprovalStatusId equals approvalStatus.Id into paStatus
                        from approvalStatus in paStatus.DefaultIfEmpty()
                        select new WeighingMachineListDto
                        {
                            Id = weighingMachine.Id,
                            Make = weighingMachine.Make,
                            Modal = weighingMachine.Modal,
                            SubPlantId = weighingMachine.SubPlantId,
                            UnitOfMeasurementId = weighingMachine.UnitOfMeasurementId,
                            WeighingMachineCode = weighingMachine.WeighingMachineCode,
                            UserEnteredPlantId = plantMaster.PlantId,
                            UserEnteredUOM = unitOfMeasurement.Name,
                            IsActive = weighingMachine.IsActive,
                            ApprovalStatusId = weighingMachine.ApprovalStatusId,
                            UserEnteredApprovalStatus = approvalStatus.ApprovalStatus
                        };
            if (!(string.IsNullOrEmpty(input.Keyword) || string.IsNullOrWhiteSpace(input.Keyword)))
            {
                query = query.Where(x => x.WeighingMachineCode.Contains(input.Keyword) ||

                                    x.UserEnteredPlantId.Contains(input.Keyword) ||
                                    x.UserEnteredUOM.Contains(input.Keyword));
            }
            if (input.SubPlantId != null)
            {
                query = query.Where(x => x.SubPlantId == input.SubPlantId);
            }
            if (!(string.IsNullOrEmpty(subPlantIdHeader) || string.IsNullOrWhiteSpace(subPlantIdHeader)))
            {
                query = query.Where(x => x.SubPlantId == Convert.ToInt32(subPlantIdHeader));
            }
            if (input.ActiveInactiveStatusId != null)
            {
                if (input.ActiveInactiveStatusId == (int)Status.In_Active)
                {
                    query = query.Where(x => !x.IsActive);
                }
                else if (input.ActiveInactiveStatusId == (int)Status.Active)
                {
                    query = query.Where(x => x.IsActive);
                }
            }
            if (input.ApprovalStatusId != null)
            {
                query = query.Where(x => x.ApprovalStatusId == input.ApprovalStatusId);
            }
            return query;
        }
        public async Task<List<WeighingMachineStampingDueOnListDto>> GetStampingDueOnWMListAsync()
        {
            var subPlantIdHeader = _httpContextAccessor.HttpContext.Request.Headers["PlantId"].FirstOrDefault();
            var stampingDueWMList = (from wm in _weighingMachineRepository.GetAll()
                                     join plant in _plantMasterRepository.GetAll()
                                     on wm.SubPlantId equals plant.Id
                                     where wm.StampingDueOn.HasValue
                                     select new WeighingMachineStampingDueOnListDto
                                     {
                                         WeighingMachineCode = wm.WeighingMachineCode,
                                         StampingDoneOn = wm.StampingDoneOn,
                                         StampingDueOn = wm.StampingDueOn,
                                         SubPlant = plant.PlantId,
                                         DueDays = (wm.StampingDueOn.Value - DateTime.Now).Days,
                                         PlantId = wm.SubPlantId
                                     });
            if (subPlantIdHeader != null && !string.IsNullOrEmpty(subPlantIdHeader))
            {
                stampingDueWMList = stampingDueWMList.Where(x => x.PlantId == Convert.ToInt32(subPlantIdHeader));
            }
            var result = await stampingDueWMList.ToListAsync();
            return result.Where(x => x.DueDays <= PMMSConsts.TotalStampingDays).ToList();
        }
    }
}