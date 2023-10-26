using Abp.Application.Services;
using Abp.Domain.Repositories;
using Abp.Linq;
using ELog.Application.CommomUtility;
using ELog.Application.CommonDto;
using ELog.Application.SelectLists.Dto;
using ELog.Application.Settings;
using ELog.ConnectorFactory;
using ELog.Core;
using ELog.Core.Authorization;
using ELog.Core.Authorization.Roles;
using ELog.Core.Authorization.Users;
using ELog.Core.Entities;
using ELog.Core.Hardware.WeighingMachine;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using static ELog.Core.PMMSEnums;

namespace ELog.Application.SelectLists
{
    [PMMSAuthorize]
    public class SelectListAppService : ApplicationService, ISelectListAppService
    {
        public IAsyncQueryableExecuter AsyncQueryableExecuter { get; set; }

        private readonly IRepository<ModeMaster> _modeRepository;
        private readonly IRepository<DesignationMaster> _designationRepository;
        private readonly IRepository<PlantMaster> _plantRepository;
        private readonly IRepository<User, long> _userRepository;
        private readonly IRepository<ApprovalStatusMaster> _approvalStatusMasterRepository;
        private readonly IRepository<Role> _roleRepository;
        private readonly IRepository<CountryMaster> _countryRepository;
        private readonly IRepository<StateMaster> _stateRepository;
        private readonly IRepository<LocationMaster> _locationMasterRepository;
        private readonly IRepository<EquipmentTypeMaster> _equipmentTypeRepository;
        private readonly IRepository<HandlingUnitTypeMaster> _handlingUnitTypeRepository;
        private readonly IRepository<SubModuleTypeMaster> _subModuleTypeMasterRepository;
        private readonly IRepository<DepartmentMaster> _departmentRepository;
        private readonly IRepository<ModuleMaster> _moduleRepository;
        private readonly IRepository<SubModuleMaster> _subModuleRepository;
        private readonly IRepository<ModuleSubModule> _moduleSubmoduleRepository;
        private readonly IRepository<UnitOfMeasurementTypeMaster> _unitOfMeasurementTypeRepository;
        private readonly IRepository<UnitOfMeasurementMaster> _unitOfMeasurementRepository;
        private readonly IRepository<CubicleMaster> _cubicleRepository;
        private readonly IRepository<DeviceTypeMaster> _deviceTypeRepository;
        private readonly IRepository<DeviceMaster> _deviceRepository;
        private readonly IRepository<AreaMaster> _areaRepository;

        private readonly IRepository<PurchaseOrder> _purchaseOrderRepository;
        private readonly IRepository<Material> _materialRepository;
        private readonly IRepository<UserPlants> _userPlantRepository;
        private readonly IRepository<CheckpointTypeMaster> _checkpointTypeRepository;
        private readonly IRepository<ChecklistTypeMaster> _checklistTypeRepository;
        private readonly IRepository<StandardWeightBoxMaster> _standardWeightBoxRepository;
        private readonly ISettingAppService _settingAppService;
        private readonly IRepository<HolidayTypeMaster> _holidayTypeRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IRepository<TransactionStatusMaster> _transactionStatusRepository;
        private readonly IRepository<InspectionChecklistMaster> _inspectionChecklistRepository;
        private readonly IRepository<InvoiceDetail> _invoiceDetailRepository;
        private readonly IRepository<WeighingMachineMaster> _weighingMachineRepository;
        private const int approvedApprovalStatusId = (int)ApprovalStatus.Approved;
        private readonly IRepository<HandlingUnitMaster> _handlingUnitRepository;
        private readonly IRepository<Palletization> _palletizationRepository;
        private readonly WeighingScaleFactory _weighingScaleFactory;
        private readonly IConfiguration _configuration;
        private const int subPlantId = (int)PlantType.SubPlant;
        private readonly IRepository<ApprovalLevelMaster> _appLevelRepository;
        private readonly IRepository<CubicleMaster> _cubicalRepository;
        private readonly IRepository<ActivityMaster> _activityRepository;
        private readonly IRepository<EquipmentMaster> _equipmentRepository;
        private readonly IRepository<MaterialMaster> _materialmasterRepository;
        private readonly IRepository<CheckpointMaster> _checkpointMasterRepository;
        private readonly IRepository<PalletMaster> _palletRepository;
        private readonly IRepository<LabelPrintPacking> _labelPrintPackingRepository;
        private readonly IRepository<ProcessOrderAfterRelease> _processOrderRepository;
        private readonly IRepository<CubicleAssignmentWIP> _cubicleAssignmentWIPRepository;
        public SelectListAppService(IRepository<ModeMaster> modeRepository, IConfiguration configuration,
           IRepository<DesignationMaster> designationRepository,
           IRepository<PlantMaster> plantRepository,
           IRepository<User, long> userRepository,
           IRepository<ApprovalStatusMaster> approvalStatusMasterRepository,
           IRepository<Role> roleRepository,
            IRepository<CountryMaster> countryRepository,
            IRepository<StateMaster> stateRepository,
            IRepository<LocationMaster> locationMasterRepository,
            IRepository<EquipmentTypeMaster> equipmentTypeRepository,
            IRepository<SubModuleTypeMaster> subModuleTypeMasterRepository,
            IRepository<ModuleMaster> moduleRepository,
            IRepository<DepartmentMaster> departmentRepository,
            IRepository<HandlingUnitTypeMaster> handlingUnitTypeRepository,
             IRepository<UnitOfMeasurementTypeMaster> unitOfMeasurementTypeRepository,
             IRepository<AreaMaster> areaRepository,

             IRepository<SubModuleMaster> subModuleRepository,
             IRepository<ModuleSubModule> moduleSubmoduleRepository,
             IRepository<UnitOfMeasurementMaster> unitOfMeasurementRepository,
             IRepository<CubicleMaster> cubicleRepository,
             IRepository<DeviceTypeMaster> deviceTypeRepository,
                  IRepository<UserPlants> userPlantRepository,
                   IRepository<CheckpointTypeMaster> checkpointTypeRepository,
                    IRepository<ChecklistTypeMaster> checklistTypeRepository,
                  ISettingAppService settingAppService, IRepository<DeviceMaster> deviceRepository,
             IRepository<PurchaseOrder> purchaseOrderRepository, IRepository<Material> materialRepository,
                  IHttpContextAccessor httpContextAccessor,
                  IRepository<TransactionStatusMaster> transactionStatusRepository, IRepository<InspectionChecklistMaster> inspectionChecklistRepository,
                  IRepository<HolidayTypeMaster> holidayTypeRepository,
                   IRepository<HandlingUnitMaster> handlingUnitRepository,
                            IRepository<Palletization> palletizationRepository,
                  IRepository<InvoiceDetail> invoiceDetailRepository,
                   IRepository<StandardWeightBoxMaster> standardWeightBoxRepository,
                   IRepository<WeighingMachineMaster> weighingMachineRepository, WeighingScaleFactory weighingScaleFactory,
                   // IRepository<StandardWeightBoxMaster> standardWeightBoxRepository,
                   IRepository<ApprovalLevelMaster> appLevelRepository,
                 IRepository<CubicleMaster> cubicalRepository,
                 IRepository<ActivityMaster> activityRepository,
                 IRepository<EquipmentMaster> equipmentRepository,
                 IRepository<MaterialMaster> materialmasterRepository,
                  IRepository<CheckpointMaster> checkpointMasterRepository,
                  IRepository<PalletMaster> palletRepository, IRepository<LabelPrintPacking> labelPrintPackingRepository,
                  IRepository<ProcessOrderAfterRelease> processOrderRepository,
                  IRepository<CubicleAssignmentWIP> cubicleAssignmentWIPRepository)
        {
            _modeRepository = modeRepository;
            _designationRepository = designationRepository;
            _plantRepository = plantRepository;
            _userRepository = userRepository;
            _approvalStatusMasterRepository = approvalStatusMasterRepository;
            _subModuleTypeMasterRepository = subModuleTypeMasterRepository;
            _roleRepository = roleRepository;
            _countryRepository = countryRepository;
            _stateRepository = stateRepository;
            _locationMasterRepository = locationMasterRepository;
            _equipmentTypeRepository = equipmentTypeRepository;
            _handlingUnitTypeRepository = handlingUnitTypeRepository;
            _departmentRepository = departmentRepository;
            _unitOfMeasurementTypeRepository = unitOfMeasurementTypeRepository;
            _unitOfMeasurementRepository = unitOfMeasurementRepository;
            _moduleRepository = moduleRepository;
            _subModuleRepository = subModuleRepository;
            _moduleSubmoduleRepository = moduleSubmoduleRepository;
            _cubicleRepository = cubicleRepository;
            _deviceTypeRepository = deviceTypeRepository;
            _areaRepository = areaRepository;

            _userPlantRepository = userPlantRepository;
            _httpContextAccessor = httpContextAccessor;
            _checkpointTypeRepository = checkpointTypeRepository;
            _checklistTypeRepository = checklistTypeRepository;
            _holidayTypeRepository = holidayTypeRepository;
            _settingAppService = settingAppService;
            _appLevelRepository = appLevelRepository;
            _activityRepository = activityRepository;
            _equipmentRepository = equipmentRepository;
            // _subModuleRepository = subModuleRepository;
            _cubicalRepository = cubicalRepository;
            _deviceRepository = deviceRepository;
            _purchaseOrderRepository = purchaseOrderRepository;
            _materialRepository = materialRepository;
            _transactionStatusRepository = transactionStatusRepository;
            _inspectionChecklistRepository = inspectionChecklistRepository;
            _invoiceDetailRepository = invoiceDetailRepository;
            _handlingUnitRepository = handlingUnitRepository;
            _palletizationRepository = palletizationRepository;
            _standardWeightBoxRepository = standardWeightBoxRepository;
            _weighingMachineRepository = weighingMachineRepository;
            _weighingScaleFactory = weighingScaleFactory;
            _configuration = configuration;
            _materialmasterRepository = materialmasterRepository;
            _checkpointMasterRepository = checkpointMasterRepository;
            _palletRepository = palletRepository;
            _labelPrintPackingRepository = labelPrintPackingRepository;
            _processOrderRepository = processOrderRepository;
            _cubicleAssignmentWIPRepository = cubicleAssignmentWIPRepository;
        }

        public async Task<List<SelectListDto>> GetModesAsync()
        {
            return await _modeRepository.GetAll().OrderBy(x => x.ModeName)
                        .Select(x => new SelectListDto { Id = x.Id, Value = x.ModeName })?
                        .ToListAsync() ?? default;
        }

        public async Task<List<SelectListDto>> GetCheckpointTypesAsync()
        {
            return await _checkpointTypeRepository.GetAll()
                        .Select(x => new SelectListDto { Id = x.Id, Value = x.Title })?
                        .ToListAsync() ?? default;
        }

        public async Task<List<SelectListDto>> GetChecklistTypesAsync(int? plantId, int subModuleId)
        {
            if (plantId != null)
            {
                return await _checklistTypeRepository.GetAll().Where(x => x.IsActive && x.SubPlantId == plantId && x.SubModuleId == subModuleId && x.ApprovalStatusId == approvedApprovalStatusId).OrderBy(x => x.ChecklistTypeCode)
                        .Select(x => new SelectListDto { Id = x.Id, Value = x.ChecklistTypeCode + " - " + x.ChecklistName })?
                        .ToListAsync() ?? default;
            }
            else
            {
                return await _checklistTypeRepository.GetAll().Where(x => x.IsActive && x.SubModuleId == subModuleId && x.ApprovalStatusId == approvedApprovalStatusId).OrderBy(x => x.ChecklistTypeCode)
                      .Select(x => new SelectListDto { Id = x.Id, Value = x.ChecklistTypeCode + " - " + x.ChecklistName })?
                      .ToListAsync() ?? default;
            }
        }

        public async Task<List<SelectListDto>> GetChecklistTypesBySubModuleNameAsync(string subModuleName)
        {
            var plantId = _httpContextAccessor.HttpContext.Request.Headers["PlantId"].FirstOrDefault();
            var subModule = subModuleName != null ? await _subModuleRepository.FirstOrDefaultAsync(a => a.Name.ToLower() == subModuleName.ToLower()) : null;
            if (subModule != null)
            {
                return !(string.IsNullOrEmpty(plantId) || string.IsNullOrWhiteSpace(plantId))
                    ? await _checklistTypeRepository.GetAll().Where(x => x.IsActive && x.SubPlantId == Convert.ToInt32(plantId) && x.SubModuleId == subModule.Id && x.ApprovalStatusId == approvedApprovalStatusId).OrderBy(x => x.ChecklistTypeCode)
                            .Select(x => new SelectListDto { Id = x.Id, Value = x.ChecklistTypeCode + " - " + x.ChecklistName })?
                            .ToListAsync() ?? default
                    : await _checklistTypeRepository.GetAll().Where(x => x.IsActive && x.SubModuleId == subModule.Id && x.ApprovalStatusId == approvedApprovalStatusId).OrderBy(x => x.ChecklistTypeCode)
                                          .Select(x => new SelectListDto { Id = x.Id, Value = x.ChecklistTypeCode + " - " + x.ChecklistName })?
                                          .ToListAsync() ?? default;
            }
            return null;
        }

        public async Task<List<SelectListDto>> GetModesBySettingAsync(int userId, bool includeIsControllerModes)
        {
            var setting = await _settingAppService.GetAll();
            var settingValue = setting.FirstOrDefault(a => a.Name == PMMSConsts.IsCommonCreatorSetting)?.Value;
            if (settingValue?.ToLower() == PMMSConsts.IsCommonCreatorSettingValue)
            {
                var qaModes = _modeRepository.GetAll().Where(a => a.IsController);
                var userModes = (from user in _userRepository.GetAll()
                                 join mode in _modeRepository.GetAll()
                                 on user.ModeId equals mode.Id
                                 where user.IsActive && user.ApprovalStatusId == approvedApprovalStatusId && user.Id == userId
                                 orderby mode.ModeName
                                 select mode);
                userModes = userModes.ToList().Count == 0 ? _modeRepository.GetAll() : userModes;
                userModes = userModes.Union(qaModes);
                return !includeIsControllerModes
                    ? userModes.Where(a => !a.IsController).Select(x => new SelectListDto { Id = x.Id, Value = x.ModeName }).ToList()
                    : userModes.Select(x => new SelectListDto { Id = x.Id, Value = x.ModeName }).ToList();
            }
            else
            {
                var allModes = _modeRepository.GetAll();
                return !includeIsControllerModes
                    ? allModes.Where(a => !a.IsController).Select(x => new SelectListDto { Id = x.Id, Value = x.ModeName }).ToList()
                    : allModes.Select(x => new SelectListDto { Id = x.Id, Value = x.ModeName }).ToList();
            }
        }

        public async Task<List<SelectListDto>> GetDesignationAsync()
        {
            return await _designationRepository.GetAll().OrderBy(x => x.DesignationName)
                      .Select(x => new SelectListDto { Id = x.Id, Value = x.DesignationName })?
                      .ToListAsync() ?? default;
        }

        public async Task<List<SelectListDto>> GetApprovalStatusAsync()
        {
            return await _approvalStatusMasterRepository.GetAll().OrderBy(x => x.ApprovalStatus)
                      .Select(x => new SelectListDto { Id = x.Id, Value = x.ApprovalStatus })?
                      .ToListAsync() ?? default;
        }

        public List<SelectListDto> GetStatus()
        {
            return Enum.GetValues(typeof(Status)).Cast<Status>().Select(v => new SelectListDto
            {
                Id = (int)v,
                Value = v.GetAttribute<DisplayAttribute>().Name
            }).OrderBy(x => x.Value).ToList();
        }

        public List<SelectListDto> GetSortByUser()
        {
            return Enum.GetValues(typeof(UsersListSortBy)).Cast<UsersListSortBy>().Select(v => new SelectListDto
            {
                Id = (int)v,
                Value = v.GetAttribute<DisplayAttribute>().Name
            }).OrderBy(x => x.Value).ToList();
        }

        public async Task<List<SelectListDto>> GetPlantAsync()
        {
            var plantId = GetLogInUserSelectedPlant();
            if (plantId != null)
            {
                return await _plantRepository.GetAll().Where(x => x.IsActive && x.ApprovalStatusId == approvedApprovalStatusId && x.Id == plantId && x.PlantTypeId == subPlantId).OrderBy(x => x.PlantId)
                                      .Select(x => new SelectListDto { Id = x.Id, Value = x.PlantId + " - " + x.PlantName })?
                                      .ToListAsync() ?? default;
            }
            else
            {
                return await GetPlantsOnUserAsync();
            }
        }

        public async Task<List<SelectListDto>> GetPlantsOnUserAsync()
        {
            long? userId = AbpSession.UserId;
            var plantId = GetLogInUserSelectedPlant();
            if (plantId != null)
            {
                var result = await (from user in _userRepository.GetAll()
                                    join userPlant in _userPlantRepository.GetAll()
                                    on user.Id equals userPlant.UserId
                                    join plant in _plantRepository.GetAll()
                                    on userPlant.PlantId equals plant.Id
                                    where user.Id == userId && plant.IsActive && plant.ApprovalStatusId == approvedApprovalStatusId
                                    && user.IsActive && plant.PlantTypeId == subPlantId
                                    orderby plant.PlantId
                                    select new SelectListDto { Id = plant.Id, Value = plant.PlantId + " - " + plant.PlantName }).ToListAsync() ?? default;

                return result;
            }
            else
            {
                return await _plantRepository.GetAll().Where(x => x.IsActive && x.ApprovalStatusId == approvedApprovalStatusId && x.PlantTypeId == subPlantId).OrderBy(x => x.PlantId)
                           .Select(x => new SelectListDto { Id = x.Id, Value = x.PlantId + " - " + x.PlantName })?
                           .ToListAsync() ?? default;
            }
        }

        public async Task<List<SelectListDto>> GetPlantByUserIdAsync(long userId)
        {
            return await (from user in _userRepository.GetAll()
                          join userPlant in _userPlantRepository.GetAll()
                          on user.Id equals userPlant.UserId
                          join plant in _plantRepository.GetAll()
                          on userPlant.PlantId equals plant.Id
                          where user.Id == userId && plant.IsActive && plant.ApprovalStatusId == approvedApprovalStatusId
                          && user.IsActive && plant.PlantTypeId == subPlantId
                          orderby plant.PlantId
                          select new SelectListDto { Id = plant.Id, Value = plant.PlantId }).ToListAsync() ?? default;
        }

        public async Task<List<SelectListDto>> GetReportingManagerUserAsync()
        {
            return await (from user in _userRepository.GetAll()
                          where (user.IsActive && user.ApprovalStatusId == approvedApprovalStatusId)
                          orderby user.Name
                          select new SelectListDto { Id = user.Id, Value = user.FullName }).ToListAsync() ?? default;
        }

        public async Task<List<SelectListDto>> GetRolesAsync()
        {
            return await _roleRepository.GetAll().Where(x => x.IsActive && x.ApprovalStatusId == approvedApprovalStatusId).OrderBy(x => x.DisplayName)
                           .Select(x => new SelectListDto { Id = x.Id, Value = x.DisplayName })?
                           .ToListAsync() ?? default;
        }

        public List<SelectListDto> GetSortByRole()
        {
            return Enum.GetValues(typeof(RoleListSortBy)).Cast<RoleListSortBy>().Select(v => new SelectListDto
            {
                Id = (int)v,
                Value = v.GetAttribute<DisplayAttribute>().Name
            }).ToList();
        }

        public List<SelectListDto> GetSortByModule()
        {
            return Enum.GetValues(typeof(ModuleListSortBy)).Cast<ModuleListSortBy>().Select(v => new SelectListDto
            {
                Id = (int)v,
                Value = v.GetAttribute<DisplayAttribute>().Name
            }).OrderBy(x => x.Value).ToList();
        }

        public List<SelectListDto> GetSortByPlant()
        {
            return Enum.GetValues(typeof(PlantListSortBy)).Cast<PlantListSortBy>().Select(v => new SelectListDto
            {
                Id = (int)v,
                Value = v.GetAttribute<DisplayAttribute>().Name
            }).OrderBy(x => x.Value).ToList();
        }

        public async Task<List<SelectListDto>> GetCountriesAsync()
        {
            return await _countryRepository.GetAll().OrderBy(x => x.CountryName)
                           .Select(x => new SelectListDto { Id = x.Id, Value = x.CountryName })?
                           .ToListAsync() ?? default;
        }

        public async Task<List<SelectListDto>> GetStatesAsync(int countryId)
        {
            return await _stateRepository.GetAll().Where(x => x.CountryId == countryId).OrderBy(x => x.StateName)
                           .Select(x => new SelectListDto { Id = x.Id, Value = x.StateName })?
                           .ToListAsync() ?? default;
        }

        public List<SelectListDto> GetSortByGate()
        {
            return Enum.GetValues(typeof(GateListSortBy)).Cast<GateListSortBy>().Select(v => new SelectListDto
            {
                Id = (int)v,
                Value = v.GetAttribute<DisplayAttribute>().Name
            }).OrderBy(x => x.Value).ToList();
        }

        public List<SelectListDto> GetSortByLocation()
        {
            return Enum.GetValues(typeof(LocationListSortBy)).Cast<LocationListSortBy>().Select(v => new SelectListDto
            {
                Id = (int)v,
                Value = v.GetAttribute<DisplayAttribute>().Name
            }).OrderBy(x => x.Value).ToList();
        }

        public async Task<List<SelectListDto>> GetAllMasterPlants()
        {
            const int masterPlantId = (int)PlantType.MasterPlant;
            return await _plantRepository.GetAll().Where(x => x.IsActive && x.ApprovalStatusId == approvedApprovalStatusId && x.PlantTypeId == masterPlantId).OrderBy(x => x.PlantId)
                      .Select(x => new SelectListDto { Id = x.Id, Value = x.PlantId + " - " + x.PlantName })?
                      .ToListAsync() ?? default;
        }

        public async Task<List<SelectListDto>> GetAllSubPlants()
        {
            return await _plantRepository.GetAll().Where(x => x.IsActive && x.ApprovalStatusId == approvedApprovalStatusId && x.PlantTypeId == subPlantId).OrderBy(x => x.PlantId)
                      .Select(x => new SelectListDto { Id = x.Id, Value = x.PlantId + " - " + x.PlantName })?
                      .ToListAsync() ?? default;
        }

        public async Task<List<SelectListDto>> GetStorageLocationsAsync()
        {
            var plantId = GetLogInUserSelectedPlant();
            if (plantId != null)
            {
                return await _locationMasterRepository.GetAll().Where(x => x.IsActive && x.ApprovalStatusId == approvedApprovalStatusId && x.PlantId == plantId).OrderBy(x => x.LocationCode)
                       .Select(x => new SelectListDto { Id = x.Id, Value = x.LocationCode })?
                       .ToListAsync() ?? default;
            }
            else
            {
                return await _locationMasterRepository.GetAll().Where(x => x.IsActive && x.ApprovalStatusId == approvedApprovalStatusId).OrderBy(x => x.LocationCode)
                        .Select(x => new SelectListDto { Id = x.Id, Value = x.LocationCode })?
                        .ToListAsync() ?? default;
            }
        }

        public async Task<List<SelectListDtoWithPlantId>> GetStorageLocationsWithPlantIdAsync()
        {
            var plantId = GetLogInUserSelectedPlant();
            if (plantId != null)
            {
                return await _locationMasterRepository.GetAll().Where(x => x.IsActive && x.ApprovalStatusId == approvedApprovalStatusId && x.PlantId == plantId).OrderBy(x => x.LocationCode)
                       .Select(x => new SelectListDtoWithPlantId { Id = x.Id, Value = x.LocationCode, PlantId = x.PlantId })?
                       .ToListAsync() ?? default;
            }
            else
            {
                return await _locationMasterRepository.GetAll().Where(x => x.IsActive && x.ApprovalStatusId == approvedApprovalStatusId).OrderBy(x => x.LocationCode)
                        .Select(x => new SelectListDtoWithPlantId { Id = x.Id, Value = x.LocationCode, PlantId = x.PlantId })?
                        .ToListAsync() ?? default;
            }
        }

        public async Task<List<SelectListDto>> GetDepartmentsAsync()
        {
            var subPlantId = GetLogInUserSelectedPlant();
            if (subPlantId != null)
            {
                return await _departmentRepository.GetAll().Where(x => x.IsActive && x.ApprovalStatusId == approvedApprovalStatusId && x.SubPlantId == subPlantId).OrderBy(x => x.DepartmentCode)
                      .Select(x => new SelectListDto { Id = x.Id, Value = x.DepartmentCode + " - " + x.DepartmentName })?
                      .ToListAsync() ?? default;
            }
            else
            {
                return await _departmentRepository.GetAll().Where(x => x.IsActive && x.ApprovalStatusId == approvedApprovalStatusId).OrderBy(x => x.DepartmentCode)
                                      .Select(x => new SelectListDto { Id = x.Id, Value = x.DepartmentCode + " - " + x.DepartmentName })?
                                      .ToListAsync() ?? default;
            }
        }

        public async Task<List<SelectListDto>> GetDepartmentsByPlantIdAsync(int plantId)
        {
            return await _departmentRepository.GetAll().Where(x => x.IsActive && x.ApprovalStatusId == approvedApprovalStatusId && x.SubPlantId == plantId).OrderBy(x => x.DepartmentCode)
                                  .Select(x => new SelectListDto { Id = x.Id, Value = x.DepartmentCode + " - " + x.DepartmentName })?
                                  .ToListAsync() ?? default;
        }

        public List<SelectListDto> GetSortByCubicle()
        {
            return Enum.GetValues(typeof(CubicleListSortBy)).Cast<CubicleListSortBy>().Select(v => new SelectListDto
            {
                Id = (int)v,
                Value = v.GetAttribute<DisplayAttribute>().Name
            }).OrderBy(x => x.Value).ToList();
        }

        public List<SelectListDto> GetSortByEquipment()
        {
            return Enum.GetValues(typeof(EquipmentListSortBy)).Cast<EquipmentListSortBy>().Select(v => new SelectListDto
            {
                Id = (int)v,
                Value = v.GetAttribute<DisplayAttribute>().Name
            }).OrderBy(x => x.Value).ToList();
        }

        public async Task<List<SelectListDto>> GetEquipmentTypesAsync()
        {
            return await _equipmentTypeRepository.GetAll().OrderBy(x => x.EquipmentName)
                      .Select(x => new SelectListDto { Id = x.Id, Value = x.EquipmentName })?
                      .ToListAsync() ?? default;
        }

        public List<SelectListDto> GetSortByHandlingUnit()
        {
            return Enum.GetValues(typeof(HandlingUnitListSortBy)).Cast<HandlingUnitListSortBy>().Select(v => new SelectListDto
            {
                Id = (int)v,
                Value = v.GetAttribute<DisplayAttribute>().Name
            }).AsQueryable().OrderBy(x => x.Value).ToList();
        }

        public async Task<List<SelectListDto>> GetHandlingUnitTypesAsync()
        {
            return await _handlingUnitTypeRepository.GetAll().OrderBy(x => x.HandlingUnitName)
                      .Select(x => new SelectListDto { Id = x.Id, Value = x.HandlingUnitName })?
                      .ToListAsync() ?? default;
        }

        public List<SelectListDto> GetSortBySubModule()
        {
            return Enum.GetValues(typeof(SubModuleListSortBy)).Cast<SubModuleListSortBy>().Select(v => new SelectListDto
            {
                Id = (int)v,
                Value = v.GetAttribute<DisplayAttribute>().Name
            }).ToList();
        }

        public List<SelectListDto> GetSortByStandardWeightBox()
        {
            return Enum.GetValues(typeof(StandardWeightBoxListSortBy)).Cast<StandardWeightBoxListSortBy>().Select(v => new SelectListDto
            {
                Id = (int)v,
                Value = v.GetAttribute<DisplayAttribute>().Name
            }).ToList();
        }

        public async Task<List<SelectListDto>> GetSubModuleTypeAsync()
        {
            return await _subModuleTypeMasterRepository.GetAll()
                      .Select(x => new SelectListDto { Id = x.Id, Value = x.SubModuleType })?
                      .ToListAsync() ?? default;
        }

        public List<SelectListDto> GetSortByStandardWeights()
        {
            return Enum.GetValues(typeof(StandardWeightListSortBy)).Cast<StandardWeightListSortBy>().Select(v => new SelectListDto
            {
                Id = (int)v,
                Value = v.GetAttribute<DisplayAttribute>().Name
            }).ToList();
        }

        public List<SelectListDto> GetSortByDepartment()
        {
            return Enum.GetValues(typeof(DepartmentListSortBy)).Cast<DepartmentListSortBy>().Select(v => new SelectListDto
            {
                Id = (int)v,
                Value = v.GetAttribute<DisplayAttribute>().Name
            }).ToList();
        }

        public List<SelectListDto> GetSortByUnitOfMeasurement()
        {
            return Enum.GetValues(typeof(UnitOfMeasurementListSortBy)).Cast<UnitOfMeasurementListSortBy>().Select(v => new SelectListDto
            {
                Id = (int)v,
                Value = v.GetAttribute<DisplayAttribute>().Name
            }).AsQueryable().OrderBy(x => x.Value).ToList();
        }

        public async Task<List<SelectListDto>> GetUnitOfMeasurementTypesAsync()
        {
            return await _unitOfMeasurementTypeRepository.GetAll().OrderBy(x => x.UnitOfMeasurementTypeName)
                      .Select(x => new SelectListDto { Id = x.Id, Value = x.UnitOfMeasurementTypeName })?
                      .ToListAsync() ?? default;
        }

        public async Task<List<SelectListDto>> GetConversionUOMMastersAsync()
        {
            return await _unitOfMeasurementRepository.GetAll().Where(x => x.IsActive && x.ApprovalStatusId == approvedApprovalStatusId).OrderBy(x => x.UOMCode)
                      .Select(x => new SelectListDto { Id = x.Id, Value = x.UOMCode + " - " + x.Name })?
                      .ToListAsync() ?? default;
        }

        public async Task<List<SelectListDto>> GetModulesAsync()
        {
            return await _moduleRepository.GetAll().Where(x => x.IsActive).OrderBy(x => x.DisplayName)
                      .Select(x => new SelectListDto { Id = x.Id, Value = x.DisplayName })?
                      .ToListAsync() ?? default;
        }

        public async Task<List<SelectListDto>> GetSubModulesAsync()
        {
            var result = await (from moduleSubModule in _moduleSubmoduleRepository.GetAll()
                                join subModule in _subModuleRepository.GetAll()
                                on moduleSubModule.SubModuleId equals subModule.Id
                                join module in _moduleRepository.GetAll()
                                on moduleSubModule.ModuleId equals module.Id
                                where subModule.IsActive && module.IsActive && module.Name != PMMSConsts.AdminModule && module.Name != PMMSConsts.MasterModule
                                orderby subModule.DisplayName
                                select new SelectListDto { Id = subModule.Id, Value = subModule.DisplayName }).ToListAsync() ?? default;

            return result;
        }

        public List<SelectListDto> GetSortByArea()
        {
            return Enum.GetValues(typeof(AreaListSortBy)).Cast<AreaListSortBy>().Select(v => new SelectListDto
            {
                Id = (int)v,
                Value = v.GetAttribute<DisplayAttribute>().Name
            }).ToList();
        }

        public List<SelectListDto> GetSortByInspectionChecklist()
        {
            return Enum.GetValues(typeof(InspectionChecklistListSortBy)).Cast<InspectionChecklistListSortBy>().Select(v => new SelectListDto
            {
                Id = (int)v,
                Value = v.GetAttribute<DisplayAttribute>().Name
            }).ToList();
        }

        public List<SelectListDto> GetSortByWeighingMachine()
        {
            return Enum.GetValues(typeof(WeighingMachineListSortBy)).Cast<WeighingMachineListSortBy>().Select(v => new SelectListDto
            {
                Id = (int)v,
                Value = v.GetAttribute<DisplayAttribute>().Name
            }).AsQueryable().OrderBy(x => x.Value).ToList();
        }

        public List<SelectListDto> GetSortByDevice()
        {
            return Enum.GetValues(typeof(DeviceListSortBy)).Cast<DeviceListSortBy>().Select(v => new SelectListDto
            {
                Id = (int)v,
                Value = v.GetAttribute<DisplayAttribute>().Name
            }).AsQueryable().OrderBy(x => x.Value).ToList();
        }

        public async Task<List<SelectListDto>> GetAreasAsync()
        {
            var subPlantId = GetLogInUserSelectedPlant();
            if (subPlantId != null)
            {
                return await _areaRepository.GetAll().Where(x => x.SubPlantId == subPlantId && x.IsActive && x.ApprovalStatusId == approvedApprovalStatusId).OrderBy(x => x.AreaCode)
                                        .Select(x => new SelectListDto { Id = x.Id, Value = x.AreaCode + " - " + x.AreaName })?
                                        .ToListAsync() ?? default;
            }
            else
            {
                return await _areaRepository.GetAll().Where(x => x.IsActive && x.ApprovalStatusId == approvedApprovalStatusId).OrderBy(x => x.AreaCode)
                                      .Select(x => new SelectListDto { Id = x.Id, Value = x.AreaCode + " - " + x.AreaName })?
                                      .ToListAsync() ?? default;
            }
        }

        public async Task<List<SelectListDto>> GetAreasByPlantIdAsync(int plantId)
        {
            return await _areaRepository.GetAll().Where(x => x.SubPlantId == plantId && x.IsActive && x.ApprovalStatusId == approvedApprovalStatusId).OrderBy(x => x.AreaCode)
                                    .Select(x => new SelectListDto { Id = x.Id, Value = x.AreaCode + " - " + x.AreaName })?
                                    .ToListAsync() ?? default;
        }

        public async Task<List<SelectListDto>> GetAreasByDepartmentIdAsync(int departmentId)
        {
            return await _areaRepository.GetAll().Where(x => x.DepartmentId == departmentId && x.IsActive && x.ApprovalStatusId == approvedApprovalStatusId).OrderBy(x => x.AreaCode)
                                    .Select(x => new SelectListDto { Id = x.Id, Value = x.AreaCode + " - " + x.AreaName })?
                                    .ToListAsync() ?? default;
        }

        public async Task<List<SelectListDto>> GetCubiclesAsync()
        {
            var plantId = GetLogInUserSelectedPlant();
            if (plantId != null)
            {
                return await _cubicleRepository.GetAll().Where(x => x.IsActive && x.ApprovalStatusId == approvedApprovalStatusId && x.PlantId == plantId).OrderBy(x => x.CubicleCode)
                                                      .Select(x => new SelectListDto { Id = x.Id, Value = x.CubicleCode })?
                                                      .ToListAsync() ?? default;
            }
            else
            {
                return await _cubicleRepository.GetAll().Where(x => x.IsActive && x.ApprovalStatusId == approvedApprovalStatusId).OrderBy(x => x.CubicleCode)
                                      .Select(x => new SelectListDto { Id = x.Id, Value = x.CubicleCode })?
                                      .ToListAsync() ?? default;
            }
        }

        public async Task<List<SelectListDto>> GetCubiclesByAreaIdAsync(int areaId)
        {
            return await _cubicleRepository.GetAll().Where(x => x.IsActive && x.ApprovalStatusId == approvedApprovalStatusId && x.AreaId == areaId).OrderBy(x => x.CubicleCode)
                                                  .Select(x => new SelectListDto { Id = x.Id, Value = x.CubicleCode })?
                                                  .ToListAsync() ?? default;
        }

        public async Task<List<SelectListDto>> GetDeviceTypesAsync()
        {
            return await _deviceTypeRepository.GetAll().OrderBy(x => x.DeviceName)
                      .Select(x => new SelectListDto { Id = x.Id, Value = x.DeviceName })?
                      .ToListAsync() ?? default;
        }

        public List<SelectListDto> GetTemperatureUnit()
        {
            return Enum.GetValues(typeof(TemperatureUnit)).Cast<TemperatureUnit>().Select(v => new SelectListDto
            {
                Id = (int)v,
                Value = v.GetAttribute<DisplayAttribute>().Name
            }).AsQueryable().OrderBy(x => x.Value).ToList();
        }

        public List<SelectListDto> GetGateEntrysortBy()
        {
            return Enum.GetValues(typeof(GateEntryListSortBy)).Cast<GateEntryListSortBy>().Select(v => new SelectListDto
            {
                Id = (int)v,
                Value = v.GetAttribute<DisplayAttribute>().Name
            }).AsQueryable().OrderBy(x => x.Value).ToList();
        }

        public List<SelectListDto> GetWeighingMachineFrequencyType()
        {
            return Enum.GetValues(typeof(WeighingMachineFrequencyType)).Cast<WeighingMachineFrequencyType>().Select(v => new SelectListDto
            {
                Id = (int)v,
                Value = v.GetAttribute<DisplayAttribute>().Name
            }).AsQueryable().OrderBy(x => x.Value).ToList();
        }

        public List<SelectListDto> GetWeighingMachineBalancedType()
        {
            return Enum.GetValues(typeof(WeighingMachineBalancedType)).Cast<WeighingMachineBalancedType>().Select(v => new SelectListDto
            {
                Id = (int)v,
                Value = v.GetAttribute<DisplayAttribute>().Name
            }).AsQueryable().OrderBy(x => x.Value).ToList();
        }

        private int? GetLogInUserSelectedPlant()
        {
            var plantId = _httpContextAccessor.HttpContext.Request.Headers["PlantId"].FirstOrDefault();
            if (!(string.IsNullOrEmpty(plantId) || string.IsNullOrWhiteSpace(plantId)))
            {
                return Convert.ToInt32(plantId);
            }
            return null;
        }

        public List<SelectListDto> GetSortByChecklistType()
        {
            return Enum.GetValues(typeof(ChecklistTypeSortBy)).Cast<ChecklistTypeSortBy>().Select(v => new SelectListDto
            {
                Id = (int)v,
                Value = v.GetAttribute<DisplayAttribute>().Name
            }).AsQueryable().OrderBy(x => x.Value).ToList();
        }

        public async Task<List<SelectListDto>> GetAssociatedPlantByUserIdAsync(long userId)
        {
            return await (from userPlant in _userPlantRepository.GetAll()
                          join plant in _plantRepository.GetAll()
                          on userPlant.PlantId equals plant.Id
                          where userPlant.UserId == userId && plant.IsActive && plant.ApprovalStatusId == approvedApprovalStatusId
                          orderby plant.PlantId
                          select new SelectListDto { Id = plant.Id, Value = plant.PlantId + " - " + plant.PlantName }).ToListAsync() ?? default;
        }

        public async Task<List<SelectListDto>> GetHolidayTypeAsync()
        {
            return await _holidayTypeRepository.GetAll().Select(x => new SelectListDto { Id = x.Id, Value = x.HolidayType })
                .OrderBy(x => x.Value)
                .ToListAsync() ?? default;
        }

        public List<SelectListDto> GetSortByCalender()
        {
            return Enum.GetValues(typeof(CalenderListSortBy)).Cast<CalenderListSortBy>().Select(v => new SelectListDto
            {
                Id = (int)v,
                Value = v.GetAttribute<DisplayAttribute>().Name
            }).AsQueryable().OrderBy(x => x.Value).ToList();
        }

        public async Task<List<SelectListDto>> GetPrintersAsync()
        {
            var result = await (from device in _deviceRepository.GetAll()
                                join type in _deviceTypeRepository.GetAll()
                                on device.DeviceTypeId equals type.Id
                                where type.DeviceName == PMMSConsts.PrinterDevice && device.IsActive
                                orderby device.DeviceId
                                select new SelectListDto { Id = device.Id, Value = device.DeviceId }).ToListAsync() ?? default;

            return result;
        }



        public async Task<List<SelectListDto>> GetTransactionStatusAsync()
        {
            return await _transactionStatusRepository.GetAll().OrderBy(x => x.TransactionStatus)
                    .Select(x => new SelectListDto { Id = x.Id, Value = x.TransactionStatus })?
                    .ToListAsync() ?? default;
        }

        public bool GetGateEntryStatus()
        {
            return _subModuleRepository.FirstOrDefault(a => a.Name == PMMSConsts.GateEntrySubModule).IsActive;
        }

        public List<SelectListDto> GetSortByVehicleInspection()
        {
            return Enum.GetValues(typeof(VehicleInspectionListSortBy)).Cast<VehicleInspectionListSortBy>().Select(v => new SelectListDto
            {
                Id = (int)v,
                Value = v.GetAttribute<DisplayAttribute>().Name
            }).AsQueryable().OrderBy(x => x.Value).ToList();
        }

        public List<SelectListDto> GetSortByMaterialInspection()
        {
            return Enum.GetValues(typeof(MaterialInspectionListSortBy)).Cast<MaterialInspectionListSortBy>().Select(v => new SelectListDto
            {
                Id = (int)v,
                Value = v.GetAttribute<DisplayAttribute>().Name
            }).AsQueryable().OrderBy(x => x.Value).ToList();
        }

        public async Task<List<SelectListDto>> GetChecklistsAsync(int checklistTypeId)
        {
            return await _inspectionChecklistRepository.GetAllIncluding(a => a.CheckpointMasters).Where(x => x.IsActive && x.ChecklistTypeId == checklistTypeId && x.ApprovalStatusId == approvedApprovalStatusId).OrderBy(x => x.ChecklistCode)
                    .Select(x => new SelectListDto { Id = x.Id, Value = x.ChecklistCode + " - " + x.Name })?
                    .ToListAsync() ?? default;
        }

        public List<SelectListDto> GetSortByWeightCapture()
        {
            return Enum.GetValues(typeof(WeightCaptureListSortBy)).Cast<WeightCaptureListSortBy>().Select(v => new SelectListDto
            {
                Id = (int)v,
                Value = v.GetAttribute<DisplayAttribute>().Name
            }).AsQueryable().OrderBy(x => x.Value).ToList();
        }

        public List<SelectListDto> GetSortByGRNPosting()
        {
            return Enum.GetValues(typeof(GrnPostingListSortBy)).Cast<GrnPostingListSortBy>().Select(v => new SelectListDto
            {
                Id = (int)v,
                Value = v.GetAttribute<DisplayAttribute>().Name
            }).AsQueryable().OrderBy(x => x.Value).ToList();
        }

        public async Task<List<SelectListDto>> GetStandardWeightBoxAsync(int plantId)
        {
            return await _standardWeightBoxRepository.GetAll().Where(x => x.IsActive && x.ApprovalStatusId == approvedApprovalStatusId && x.SubPlantId == plantId).OrderBy(x => x.StandardWeightBoxId)
                        .Select(x => new SelectListDto { Id = x.Id, Value = x.StandardWeightBoxId })?
                        .ToListAsync() ?? default;
        }

        public async Task<List<SelectListDto>> GetInvoiceByPurchaseOrderIdAsync(int purchaseorderId)
        {
            return await _invoiceDetailRepository.GetAll().Where(x => x.PurchaseOrderId == purchaseorderId).OrderBy(x => x.InvoiceNo)
                    .Select(x => new SelectListDto { Id = x.Id, Value = x.InvoiceNo })?
                    .ToListAsync() ?? default;
        }
        public async Task<List<SelectListDto>> GetUnitOfMeasurementByIdAsync(int uomId)
        {
            return await _unitOfMeasurementRepository.GetAll().Where(x => x.IsActive && x.Id == uomId && x.ApprovalStatusId == approvedApprovalStatusId).OrderBy(x => x.Name)
                                  .Select(x => new SelectListDto { Id = x.Id, Value = x.UOMCode + " - " + x.Name })?
                                  .ToListAsync() ?? default;
        }

        public List<SelectListDto> GetSortByPalletization()
        {
            return Enum.GetValues(typeof(PalletizationListSortBy)).Cast<PalletizationListSortBy>().Select(v => new SelectListDto
            {
                Id = (int)v,
                Value = v.GetAttribute<DisplayAttribute>().Name
            }).AsQueryable().OrderBy(x => x.Value).ToList();
        }

        public async Task<List<SelectListDtoWithPlantId>> GetAllMaterialSelectListAsync()
        {
            var plantId = _httpContextAccessor.HttpContext.Request.Headers["PlantId"].FirstOrDefault();
            var materialQuery = (from material in _materialRepository.GetAll()
                                 join purchaseOrder in _purchaseOrderRepository.GetAll()
                                 on material.PurchaseOrderId equals purchaseOrder.Id
                                 orderby material.ItemCode
                                 select new SelectListDtoWithPlantId
                                 {
                                     Id = material.Id,
                                     Value = material.ItemNo + "-" + material.ItemCode,
                                     PlantId = purchaseOrder.PlantId
                                 });

            if (!(string.IsNullOrEmpty(plantId) || string.IsNullOrWhiteSpace(plantId)))
            {
                materialQuery = materialQuery.Where(x => x.PlantId == Convert.ToInt32(plantId));
            }
            return await materialQuery.ToListAsync() ?? default;
        }

        public async Task<List<SelectListDtoWithPlantId>> GetAllPalletsAsync()
        {
            var plantId = _httpContextAccessor.HttpContext.Request.Headers["PlantId"].FirstOrDefault();
            var handlingUnitQuery = _handlingUnitRepository.GetAll().OrderBy(x => x.Name)
                      .Select(x => new SelectListDtoWithPlantId { Id = x.Id, Value = x.HUCode + " - " + x.Name, PlantId = x.PlantId, IsActive = x.IsActive });
            if (!(string.IsNullOrEmpty(plantId) || string.IsNullOrWhiteSpace(plantId)))
            {
                handlingUnitQuery = handlingUnitQuery.Where(x => x.PlantId == Convert.ToInt32(plantId));
            }
            return await handlingUnitQuery.Where(x => x.Value != null).ToListAsync() ?? default;
        }

        public async Task<List<SelectListDtoWithPlantIdPalletization>> GetAllPalletizationPalletsAsync()
        {
            var plantId = _httpContextAccessor.HttpContext.Request.Headers["PlantId"].FirstOrDefault();
            var handlingUnitQuery = from unit in _handlingUnitRepository.GetAll()
                                    join pallet in _palletizationRepository.GetAll()
                                    on unit.Id equals pallet.PalletId into p
                                    from pal in p.DefaultIfEmpty()
                                    where pal.IsDeleted == false && unit.IsActive == true && unit.IsDeleted == false
                                    orderby pal.CreationTime ascending
                                    select new SelectListDtoWithPlantIdPalletization
                                    {
                                        Id = unit.Id,
                                        Value = unit.HUCode + " - " + unit.Name,
                                        HUCode = unit.HUCode,
                                        Name = unit.Name,
                                        PlantId = unit.PlantId,
                                        IsActive = unit.IsActive,
                                        ProductBatchNo = pal.ProductBatchNo,
                                        ContainerBarCode = pal.ContainerBarCode
                                    };

            if (!(string.IsNullOrEmpty(plantId) || string.IsNullOrWhiteSpace(plantId)))
            {
                handlingUnitQuery = handlingUnitQuery.Where(x => x.PlantId == Convert.ToInt32(plantId));
            }
            return await AsyncQueryableExecuter.ToListAsync(handlingUnitQuery);
        }

        public async Task<List<SelectListDtoWithPlantIdPalletization>> GetAllPalletMasterPalletsAsync()
        {
            var plantId = _httpContextAccessor.HttpContext.Request.Headers["PlantId"].FirstOrDefault();
            var handlingUnitQuery = from unit in _handlingUnitRepository.GetAll()
                                    join pallet in _palletRepository.GetAll()
                                    on unit.HUCode equals pallet.Pallet_Barcode into p
                                    from pal in p.DefaultIfEmpty()
                                    where pal.IsDeleted == false && unit.IsActive == true && unit.IsDeleted == false
                                    orderby pal.CreationTime ascending
                                    select new SelectListDtoWithPlantIdPalletization
                                    {
                                        Id = unit.Id,
                                        Value = pal.Pallet_Barcode,
                                        HUCode = unit.HUCode,
                                        Name = unit.Name,
                                        PlantId = unit.PlantId,
                                        IsActive = unit.IsActive,
                                        ProductBatchNo = pal.ProductBatchNo
                                    };

            if (!(string.IsNullOrEmpty(plantId) || string.IsNullOrWhiteSpace(plantId)))
            {
                handlingUnitQuery = handlingUnitQuery.Where(x => x.PlantId == Convert.ToInt32(plantId));
            }
            return await AsyncQueryableExecuter.ToListAsync(handlingUnitQuery);
        }

        public List<SelectListDto> GetSortByPutAway()
        {
            return Enum.GetValues(typeof(PutAwayListSortBy)).Cast<PutAwayListSortBy>().Select(v => new SelectListDto
            {
                Id = (int)v,
                Value = v.GetAttribute<DisplayAttribute>().Name
            }).AsQueryable().OrderBy(x => x.Value).ToList();
        }
        public List<SelectListDto> GetSortByCubicleAssignment()
        {
            return Enum.GetValues(typeof(CubicleAssignmentListSortBy)).Cast<CubicleAssignmentListSortBy>().Select(v => new SelectListDto
            {
                Id = (int)v,
                Value = v.GetAttribute<DisplayAttribute>().Name
            }).AsQueryable().OrderBy(x => x.Value).ToList();
        }
        public List<SelectListDto> GetSortBySamplingCubicleAssignment()
        {
            return Enum.GetValues(typeof(SamplingCubicleAssignmentListSortBy)).Cast<SamplingCubicleAssignmentListSortBy>().Select(v => new SelectListDto
            {
                Id = (int)v,
                Value = v.GetAttribute<DisplayAttribute>().Name
            }).AsQueryable().OrderBy(x => x.Value).ToList();
        }
        public async Task<List<SelectListDtoWithPlantId>> GetAllCubicleBarcodeAsync()
        {
            var plantId = _httpContextAccessor.HttpContext.Request.Headers["PlantId"].FirstOrDefault();
            var cubicleQuery = from cubicle in _cubicleRepository.GetAll()
                               where cubicle.ApprovalStatusId == approvedApprovalStatusId && cubicle.IsActive
                               select new SelectListDtoWithPlantId
                               {
                                   Id = cubicle.Id,
                                   Value = cubicle.CubicleCode,
                                   PlantId = cubicle.PlantId,
                               };
            if (!(string.IsNullOrEmpty(plantId) || string.IsNullOrWhiteSpace(plantId)))
            {
                cubicleQuery = cubicleQuery.Where(x => x.PlantId == Convert.ToInt32(plantId));
            }
            return await cubicleQuery.ToListAsync() ?? default;
        }
        public List<SelectListDto> GetCubicleAssignmentGroupStatus()
        {
            return Enum.GetValues(typeof(CubicleAssignmentGroupStatus)).Cast<CubicleAssignmentGroupStatus>().Select(v => new SelectListDto
            {
                Id = (int)v,
                Value = v.GetAttribute<DisplayAttribute>().Name
            }).OrderBy(x => x.Value).ToList();
        }

        public List<SelectListDto> GetCalibrationStatusTest()
        {
            return Enum.GetValues(typeof(CalibrationTestStatus)).Cast<CalibrationTestStatus>().Select(v => new SelectListDto
            {
                Id = (int)v,
                Value = v.GetAttribute<DisplayAttribute>().Name
            }).ToList();
        }

        public List<SelectListDto> GetCalibrationStatus()
        {
            return Enum.GetValues(typeof(CalibrationStatus)).Cast<CalibrationStatus>().Select(v => new SelectListDto
            {
                Id = (int)v,
                Value = v.GetAttribute<DisplayAttribute>().Name
            }).ToList();
        }

        public List<SelectListDto> GetSortByWeighingCalibration()
        {
            return Enum.GetValues(typeof(WeighingCalibrationListSortBy)).Cast<WeighingCalibrationListSortBy>().Select(v => new SelectListDto
            {
                Id = (int)v,
                Value = v.GetAttribute<DisplayAttribute>().Name
            }).ToList();
        }
        public async Task<List<SelectListDtoWithPlantId>> GetmaterialsByPurchaseOrder(List<int?> purchaseorderIdList, int? plantId)
        {
            var materialQuery = (from material in _materialRepository.GetAll()
                                 join purchaseOrder in _purchaseOrderRepository.GetAll()
                                 on material.PurchaseOrderId equals purchaseOrder.Id
                                 orderby material.ItemCode
                                 select new
                                 {
                                     Id = material.Id,
                                     Value = material.ItemNo + "-" + material.ItemCode,
                                     PurchaseorderId = material.PurchaseOrderId,
                                     plantId = purchaseOrder.PlantId,
                                 });
            if (plantId != null)
            {
                materialQuery = materialQuery.Where(x => x.plantId == plantId);
            }
            if (purchaseorderIdList != null && purchaseorderIdList.Any())
            {
                materialQuery = materialQuery.Where(x => purchaseorderIdList.Contains(x.PurchaseorderId));
            }
            var materialSelectList = await materialQuery.Select(a => new SelectListDtoWithPlantId { Id = a.Id, Value = a.Value, PlantId = a.plantId }).ToListAsync();
            return materialSelectList.GroupBy(x => new { x.Value, x.PlantId }).Select(x => x.First()).ToList() ?? default;
        }
        
       

        public async Task<List<SelectListDto>> GetAllUsers()
        {
            return await _userRepository.GetAll().Where(x => x.IsActive).OrderBy(x => x.UserName)
                      .Select(x => new SelectListDto { Id = x.Id, Value = x.UserName })?
                      .ToListAsync() ?? default;
        }

        public async Task<List<SelectListDto>> GetAllApprovelLevels()
        {
            return await _appLevelRepository.GetAll().Where(x => x.IsActive).OrderBy(x => x.LevelCode)
                      .Select(x => new SelectListDto { Id = x.LevelCode, Value = x.LevelCode.ToString() })?
                      .ToListAsync() ?? default;
        }

        public async Task<List<SelectListDto>> GetAllSubModules()
        {
            return await _subModuleRepository.GetAll().Where(x => x.IsActive).OrderBy(x => x.DisplayName)
                      .Select(x => new SelectListDto { Id = x.Id, Value = x.DisplayName })?
                      .ToListAsync() ?? default;
        }

        public async Task<List<SelectListDto>> GetAllcubicals()
        {
            return await _cubicalRepository.GetAll().Where(x => x.IsActive).OrderBy(x => x.CubicleCode)
                      .Select(x => new SelectListDto { Id = x.Id, Value = x.CubicleCode })?
                      .ToListAsync() ?? default;
        }
        public async Task<List<SelectListDto>> GetAllcubicalsOfCurrentPlantId()
        {
            var plantId = GetLogInUserSelectedPlant();
            if (plantId == null)
            {
                return await _cubicalRepository.GetAll().Where(x => x.IsActive).OrderBy(x => x.CubicleCode)
                     .Select(x => new SelectListDto { Id = x.Id, Value = x.CubicleCode })?
                     .ToListAsync() ?? default;
            }
            else
            {
                return await _cubicalRepository.GetAll().Where(x => x.IsActive && x.PlantId == plantId).OrderBy(x => x.CubicleCode)
                     .Select(x => new SelectListDto { Id = x.Id, Value = x.CubicleCode })?
                     .ToListAsync() ?? default;
            }

        }

        public async Task<List<SelectListDto>> GetAllActivity(string subModule)
        {
            return await (from activity in _activityRepository.GetAll().Where(x => x.IsActive).OrderBy(x => x.ActivityCode)
                          join module in _moduleRepository.GetAll()
                          on activity.ModuleId equals module.Id
                          join submodule in _subModuleRepository.GetAll()
                          on activity.SubModuleId equals submodule.Id
                          where module.Name == "WIP" && module.IsActive && submodule.IsActive
                          && submodule.Name == subModule
                          select new SelectListDto { Id = activity.Id, Value = activity.ActivityName }).ToListAsync() ?? default;
            //return await _activityRepository.GetAll().Where(x => x.IsActive).OrderBy(x => x.ActivityCode)
            //          .Select(x => new SelectListDto { Id = x.Id, Value = x.ActivityCode })?
            //          .ToListAsync() ?? default;
        }

        public async Task<List<SelectListDto>> GetAllEquipments()
        {
            return await _equipmentRepository.GetAll().Where(x => x.IsActive && x.ApprovalStatusId == approvedApprovalStatusId).OrderBy(x => x.EquipmentCode)
                                                  .Select(x => new SelectListDto { Id = x.Id, Value = x.EquipmentCode })?
                                                  .ToListAsync() ?? default;
        }

        public async Task<List<SelectListDto>> GetMaterialMasterAsync()
        {
            return await _materialmasterRepository.GetAll().OrderBy(x => x.MaterialCode)
                    .Select(x => new SelectListDto { Id = x.Id, Value = x.MaterialCode })?
                    .ToListAsync() ?? default;
        }



        public List<SelectListDto> GetSortByPasswordRequestedUsers()
        {
            return Enum.GetValues(typeof(PasswordListSortBy)).Cast<PasswordListSortBy>().Select(v => new SelectListDto
            {
                Id = (int)v,
                Value = v.GetAttribute<DisplayAttribute>().Name
            }).ToList();
        }
        public async Task<List<SelectListDtoWithPlantId>> GetAllCheckList(string subModule)
        {
            var plantId = _httpContextAccessor.HttpContext.Request.Headers["PlantId"].FirstOrDefault();
            var checklist = (from activity in _checklistTypeRepository.GetAll().Where(x => x.IsActive && x.ApprovalStatusId == approvedApprovalStatusId).OrderBy(x => x.ChecklistName)
                             join submodule in _subModuleRepository.GetAll()
                             on activity.SubModuleId equals submodule.Id
                             where submodule.IsActive
                             && submodule.Name == subModule
                             select new SelectListDtoWithPlantId { Id = activity.Id, Value = activity.ChecklistName, PlantId = activity.SubPlantId });

            if (!(string.IsNullOrEmpty(plantId) || string.IsNullOrWhiteSpace(plantId)))
            {
                checklist = checklist.Where(x => x.PlantId == Convert.ToInt32(plantId));
            }
            return await checklist.ToListAsync() ?? default;

        }

        public async Task<List<SelectListDtoWithPlantId>> GetAllInspectionCheckList(string subModule)
        {
            var plantId = _httpContextAccessor.HttpContext.Request.Headers["PlantId"].FirstOrDefault();
            var checklist = (from activity in _inspectionChecklistRepository.GetAll().Where(x => x.IsActive && x.ApprovalStatusId == approvedApprovalStatusId).OrderBy(x => x.Name)
                             join submodule in _subModuleRepository.GetAll()
                             on activity.SubModuleId equals submodule.Id
                             where submodule.IsActive
                             && submodule.Name == subModule
                             select new SelectListDtoWithPlantId { Id = activity.Id, Value = activity.Name, PlantId = activity.PlantId });

            if (!(string.IsNullOrEmpty(plantId) || string.IsNullOrWhiteSpace(plantId)))
            {
                checklist = checklist.Where(x => x.PlantId == Convert.ToInt32(plantId));
            }
            return await checklist.ToListAsync() ?? default;

        }

        public async Task<List<SelectListDto>> GetAllPalletCode()
        {
            return await _handlingUnitRepository.GetAll().Where(x => x.HUCode != null && x.ApprovalStatusId == approvedApprovalStatusId)
                       .Select(x => new SelectListDto { Id = x.Id, Value = x.HUCode })?
                       .ToListAsync() ?? default;

        }
        public async Task<List<SelectListDto>> GetAllShipperCode()
        {
            return await _labelPrintPackingRepository.GetAll().Where(x => x.PackingLabelBarcode != null)
                       .Select(x => new SelectListDto { Id = x.Id, Value = x.PackingLabelBarcode })?
                       .ToListAsync() ?? default;

        }
        public async Task<List<SelectListDto>> GetProcessOrdersAssignedToCubicleAsync()
        {
            var processOrders = await (from processOrder in _processOrderRepository.GetAll()
                                       from cubicleAssignement in _cubicleAssignmentWIPRepository.GetAll()
                                        .Where(x => x.ProcessOrderId == processOrder.Id).Take(1)
                                       select new SelectListDto
                                       {
                                           Id = cubicleAssignement.ProcessOrderId,
                                           Value = processOrder.ProcessOrderNo,
                                       }).ToListAsync();

            return processOrders;

        }
        public async Task<List<SelectListDto>> GetPlantCode()
        {
            List<SelectListDto> value = new List<SelectListDto>();

            try
            {
                string connection = _configuration["ConnectionStrings:Default"];
                MySqlConnection conn = new MySqlConnection(connection);
                MySqlDataReader myReader = null;
                DataTable dt = new DataTable();
                using (MySqlCommand Command = new MySqlCommand())
                {
                    Command.Connection = conn;

                    Command.CommandText = Constants.Schema + Constants.SP_SelectList;
                    Command.Parameters.Add(Constants.Type, MySqlDbType.VarChar).Value = Constants.GetPlantCode;
                    Command.CommandType = CommandType.StoredProcedure;
                    Command.Connection.Open();
                    myReader = await Command.ExecuteReaderAsync();
                    dt.Load(myReader);
                    Command.Connection.Close();
                }

                foreach (DataRow dtRow in dt.Rows)
                {
                    //// On all tables' columns
                    //foreach (DataColumn dc in dt.Columns)
                    //{
                    //    var field1 = dtRow[dc].ToString();

                    //}

                    SelectListDto selectListDto = new SelectListDto();
                    selectListDto.Id = Convert.ToString(dtRow["code"]);
                    selectListDto.Value = Convert.ToString(dtRow["code"]);

                    value.Add(selectListDto);
                    //var result = Utility.ToListof<SelectListDto>(dt);

                }
                return value;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return null;

        }

        public async Task<Object> GetPackingOrderNo(string plantCode)
        {
            List<SelectListDto> value = new List<SelectListDto>();

            try
            {
                string connection = _configuration["ConnectionStrings:Default"];
                MySqlConnection conn = new MySqlConnection(connection);
                MySqlDataReader myReader = null;
                DataTable dt = new DataTable();
                using (MySqlCommand Command = new MySqlCommand())
                {
                    Command.Connection = conn;

                    Command.CommandText = Constants.SP_GenerateSerialNumber;
                    Command.Parameters.Add("sType", MySqlDbType.VarChar).Value = Constants.GetPackingOrder;
                    Command.Parameters.Add("sLineCode", MySqlDbType.VarChar).Value = String.Empty;
                    Command.Parameters.Add("sPlantCode", MySqlDbType.VarChar).Value = plantCode;
                    Command.Parameters.Add("sPackingOrderNo", MySqlDbType.VarChar).Value = String.Empty;
                    Command.Parameters.Add("sUserId", MySqlDbType.VarChar).Value = String.Empty;
                    Command.Parameters.Add("sSupplierCode", MySqlDbType.VarChar).Value = String.Empty;
                    Command.Parameters.Add("sDriverCode", MySqlDbType.VarChar).Value = String.Empty;
                    Command.Parameters.Add("sQuantity", MySqlDbType.Double).Value = 0;
                    Command.Parameters.Add("sPackingDate", MySqlDbType.Date).Value = default;
                    Command.Parameters.Add("sItemCode", MySqlDbType.VarChar).Value = String.Empty;
                    Command.Parameters.Add("sPrintedQty", MySqlDbType.Double).Value = 0;
                    Command.Parameters.Add("sPendingQtyToPrint", MySqlDbType.Double).Value = 0;
                    Command.Parameters.Add("sPrintingQty", MySqlDbType.Double).Value = 0;
                    Command.CommandType = CommandType.StoredProcedure;
                    await Command.Connection.OpenAsync();
                    myReader = await Command.ExecuteReaderAsync();
                    dt.Load(myReader);
                    await Command.Connection.CloseAsync();
                }
                foreach (DataRow dtRow in dt.Rows)
                {
                    SelectListDto selectListDto = new SelectListDto();
                    selectListDto.Id = Convert.ToString(dtRow["PackingOrderNo"]);
                    selectListDto.Value = Convert.ToString(dtRow["PackingOrderNo"]);
                    value.Add(selectListDto);
                }
                
                return value;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return null;

        }


        public async Task<Object> GetLineWorkNo()
        {
            List<SelectListDto> value = new List<SelectListDto>();

                try
                {
                    string connection = _configuration["ConnectionStrings:Default"];
                    MySqlConnection conn = new MySqlConnection(connection);
                    MySqlDataReader myReader = null;
                    DataTable dt = new DataTable();
                    using (MySqlCommand Command = new MySqlCommand())
                    {
                        Command.Connection = conn;

                        Command.CommandText =  Constants.SP_SelectList;
                        Command.Parameters.Add(Constants.Type, MySqlDbType.VarChar).Value = Constants.GetLineCode;
                        Command.CommandType = CommandType.StoredProcedure;
                        Command.Connection.Open();
                        myReader = await Command.ExecuteReaderAsync();
                        dt.Load(myReader);
                        Command.Connection.Close();
                    }

                    foreach (DataRow dtRow in dt.Rows)
                    {
                        //// On all tables' columns
                        //foreach (DataColumn dc in dt.Columns)
                        //{
                        //    var field1 = dtRow[dc].ToString();

                        //}

                        SelectListDto selectListDto = new SelectListDto();
                        selectListDto.Id = Convert.ToString(dtRow["workcentercode"]);
                        selectListDto.Value = Convert.ToString(dtRow["workcentercode"]);

                        value.Add(selectListDto);
                        //var result = Utility.ToListof<SelectListDto>(dt);

                    }
                    return value;
                }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return null;

        }

        public async Task<Object> GetPackingOrder()
        {
            List<SelectListDto> value = new List<SelectListDto>();

            try
            {
                string connection = _configuration["ConnectionStrings:Default"];
                MySqlConnection conn = new MySqlConnection(connection);
                MySqlDataReader myReader = null;
                DataTable dt = new DataTable();
                using (MySqlCommand Command = new MySqlCommand())
                {
                    Command.Connection = conn;
                    Command.CommandText =  Constants.sp_QualitySampling;
                    Command.Parameters.Add("sType", MySqlDbType.VarChar).Value = Constants.GetPackingOrder;
                    //Command.Parameters.Add("sPlantCode", MySqlDbType.VarChar).Value = PlantCode;
                    //Command.Parameters.Add("sLineCode", MySqlDbType.VarChar).Value = LineNo;
                    Command.Parameters.Add("sCartonBarCode", MySqlDbType.VarChar).Value = String.Empty;
                    Command.Parameters.Add("sChildBarCode", MySqlDbType.VarChar).Value = String.Empty;
                    Command.Parameters.Add("sPackingOrderNo", MySqlDbType.VarChar).Value = String.Empty;
                    Command.Parameters.Add("sUserId", MySqlDbType.VarChar).Value = AbpSession.UserId;

                    Command.CommandText = Constants.Schema + Constants.SP_SelectList;
                    Command.Parameters.Add(Constants.Type, MySqlDbType.VarChar).Value = Constants.GetPackingOrder;

                    Command.CommandType = CommandType.StoredProcedure;
                    Command.Connection.Open();
                    myReader = await Command.ExecuteReaderAsync();
                    dt.Load(myReader);
                    Command.Connection.Close();
                }

                foreach (DataRow dtRow in dt.Rows)
                {
                   

                    SelectListDto selectListDto = new SelectListDto();
                    selectListDto.Id = Convert.ToString(dtRow["packingorderno"]);
                    selectListDto.Value = Convert.ToString(dtRow["packingorderno"]);
                    value.Add(selectListDto);
                   

                }
                return value;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return null;

        }
       
    }
}