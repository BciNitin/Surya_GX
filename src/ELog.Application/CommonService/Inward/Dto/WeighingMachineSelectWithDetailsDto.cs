using Abp.Application.Services.Dto;

namespace ELog.Application.CommonService.Inward.Dto
{
    public class WeighingMachineSelectWithDetailsDto : EntityDto<int>
    {
        public string WeighingMachineCode { get; set; }
        public string IPAddress { get; set; }

        public int? PortNumber { get; set; }
        public int? SubPlantId { get; set; }
        public double? Weight { get; set; }
        public int? LeastCountDigitAfterDecimal { get; set; }
    }
}