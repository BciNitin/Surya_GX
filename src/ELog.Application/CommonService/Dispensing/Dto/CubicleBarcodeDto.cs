using Abp.Application.Services.Dto;

namespace ELog.Application.CommonService.Dispensing.Dto
{
    public class CubicleBarcodeDto : EntityDto<int>
    {
        public string Value { get; set; }
        public int? PlantId { get; set; }
        public string AreaCode { get; set; }
    }
}
