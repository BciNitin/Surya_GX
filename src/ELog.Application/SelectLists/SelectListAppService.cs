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
        private readonly IRepository<PlantMaster> _plantRepository;
        private readonly IRepository<User, long> _userRepository;
        private readonly IRepository<ApprovalStatusMaster> _approvalStatusMasterRepository;
        private readonly IRepository<Role> _roleRepository;
        private readonly IRepository<SubModuleTypeMaster> _subModuleTypeMasterRepository;
        private readonly IRepository<ModuleMaster> _moduleRepository;
        private readonly IRepository<SubModuleMaster> _subModuleRepository;
        private readonly IRepository<ModuleSubModule> _moduleSubmoduleRepository;
        private readonly IRepository<UserPlants> _userPlantRepository;
        private readonly IRepository<CheckpointTypeMaster> _checkpointTypeRepository;
        private readonly IRepository<ChecklistTypeMaster> _checklistTypeRepository;
        private readonly ISettingAppService _settingAppService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private const int approvedApprovalStatusId = (int)ApprovalStatus.Approved;
        private readonly WeighingScaleFactory _weighingScaleFactory;
        private readonly IConfiguration _configuration;
        private const int subPlantId = (int)PlantType.SubPlant;
        private readonly IRepository<ApprovalLevelMaster> _appLevelRepository;
        private readonly IRepository<ActivityMaster> _activityRepository;
        private readonly IRepository<CheckpointMaster> _checkpointMasterRepository;
        private readonly IRepository<PalletMaster> _palletRepository;
        public SelectListAppService(IRepository<ModeMaster> modeRepository, IConfiguration configuration,
           IRepository<PlantMaster> plantRepository,
           IRepository<User, long> userRepository,
           IRepository<ApprovalStatusMaster> approvalStatusMasterRepository,
           IRepository<Role> roleRepository,
            IRepository<SubModuleTypeMaster> subModuleTypeMasterRepository,
            IRepository<ModuleMaster> moduleRepository,

             IRepository<SubModuleMaster> subModuleRepository,
             IRepository<ModuleSubModule> moduleSubmoduleRepository,
                  IRepository<UserPlants> userPlantRepository,
                   IRepository<CheckpointTypeMaster> checkpointTypeRepository,
                    IRepository<ChecklistTypeMaster> checklistTypeRepository,
                  ISettingAppService settingAppService, 
                  IHttpContextAccessor httpContextAccessor,
                   WeighingScaleFactory weighingScaleFactory,
                   // IRepository<StandardWeightBoxMaster> standardWeightBoxRepository,
                   IRepository<ApprovalLevelMaster> appLevelRepository,
                 IRepository<ActivityMaster> activityRepository,
                  IRepository<CheckpointMaster> checkpointMasterRepository,
                  IRepository<PalletMaster> palletRepository)
        {
            _modeRepository = modeRepository;
            _plantRepository = plantRepository;
            _userRepository = userRepository;
            _approvalStatusMasterRepository = approvalStatusMasterRepository;
            _subModuleTypeMasterRepository = subModuleTypeMasterRepository;
            _roleRepository = roleRepository;
            _moduleRepository = moduleRepository;
            _subModuleRepository = subModuleRepository;
            _moduleSubmoduleRepository = moduleSubmoduleRepository;

            _userPlantRepository = userPlantRepository;
            _httpContextAccessor = httpContextAccessor;
            _checkpointTypeRepository = checkpointTypeRepository;
            _checklistTypeRepository = checklistTypeRepository;
            _settingAppService = settingAppService;
            _appLevelRepository = appLevelRepository;
            _activityRepository = activityRepository;
            // _subModuleRepository = subModuleRepository;
            _weighingScaleFactory = weighingScaleFactory;
            _configuration = configuration;
            _checkpointMasterRepository = checkpointMasterRepository;
            _palletRepository = palletRepository;
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

      

        public List<SelectListDto> GetSortByHandlingUnit()
        {
            return Enum.GetValues(typeof(HandlingUnitListSortBy)).Cast<HandlingUnitListSortBy>().Select(v => new SelectListDto
            {
                Id = (int)v,
                Value = v.GetAttribute<DisplayAttribute>().Name
            }).AsQueryable().OrderBy(x => x.Value).ToList();
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


        public List<SelectListDto> GetSortByCalender()
        {
            return Enum.GetValues(typeof(CalenderListSortBy)).Cast<CalenderListSortBy>().Select(v => new SelectListDto
            {
                Id = (int)v,
                Value = v.GetAttribute<DisplayAttribute>().Name
            }).AsQueryable().OrderBy(x => x.Value).ToList();
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

      

        public List<SelectListDto> GetSortByPalletization()
        {
            return Enum.GetValues(typeof(PalletizationListSortBy)).Cast<PalletizationListSortBy>().Select(v => new SelectListDto
            {
                Id = (int)v,
                Value = v.GetAttribute<DisplayAttribute>().Name
            }).AsQueryable().OrderBy(x => x.Value).ToList();
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