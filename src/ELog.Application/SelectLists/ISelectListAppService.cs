using Abp.Application.Services;

using ELog.Application.CommonDto;
using ELog.Application.SelectLists.Dto;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace ELog.Application.SelectLists
{
    public interface ISelectListAppService : IApplicationService
    {
        Task<List<SelectListDto>> GetModesAsync();

        Task<List<SelectListDto>> GetApprovalStatusAsync();

        List<SelectListDto> GetSortByUser();

        Task<List<SelectListDto>> GetPlantAsync();

        Task<List<SelectListDto>> GetPlantByUserIdAsync(long userId);

        Task<List<SelectListDto>> GetReportingManagerUserAsync();

        Task<List<SelectListDto>> GetRolesAsync();

        List<SelectListDto> GetSortByRole();

        List<SelectListDto> GetStatus();

        List<SelectListDto> GetSortByPlant();

        List<SelectListDto> GetSortByGate();

        List<SelectListDto> GetSortByLocation();

        Task<List<SelectListDto>> GetAllSubPlants();

        List<SelectListDto> GetSortByEquipment();

        Task<List<SelectListDto>> GetSubModulesAsync();

        Task<List<SelectListDto>> GetModulesAsync();

        Task<List<SelectListDto>> GetSubModuleTypeAsync();

        List<SelectListDto> GetSortBySubModule();

        Task<List<SelectListDto>> GetAssociatedPlantByUserIdAsync(long userId);

        bool GetGateEntryStatus();


        Task<List<SelectListDto>> GetAllUsers();
        Task<List<SelectListDto>> GetAllApprovelLevels();
        Task<List<SelectListDto>> GetAllSubModules();
        Task<List<SelectListDto>> GetAllActivity(string subModule);

        //List<SelectListDto> getSortByWeighingCalibration();
        
    }
}