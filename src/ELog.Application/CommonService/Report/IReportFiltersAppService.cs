using Abp.Application.Services;

using ELog.Application.CommonDto;
using ELog.Application.SelectLists.Dto;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ELog.Application.CommonService.Report
{
    public interface IReportFiltersAppService : IApplicationService
    {
        Task<List<SelectListDtoWithPlantId>> GetAssignCubiclesByPlantIdAsync(int? plantId);
        Task<List<SelectListDtoWithPlantId>> GetCubiclesAssignProductsAsync(int? plantId);
        Task<List<SelectListDtoWithPlantId>> GetCubiclesAssignSAPBatchNoAsync(int? plantId);
        Task<List<SelectListDtoWithPlantId>> GetLineClearanceCubicleByPlantIdAsync(int? plantId);
        Task<List<SelectListDtoWithPlantId>> GetAllLineClearanceMaterialAsync(int? plantId);
        Task<List<SelectListDtoWithPlantId>> GetAllCleanCubicleByPlantAsync(int? plantId);
        Task<List<SelectListDtoWithPlantId>> GetAllCleanEquipmentByPlantAsync(int? plantId);
        List<SelectListDto> GetCalibrationModeAsync();
        Task<string> GetLastCalibrationStatus(DateTime calibrationDate, int? weighingMachineId, int calibrationHeaderId);
    }
}