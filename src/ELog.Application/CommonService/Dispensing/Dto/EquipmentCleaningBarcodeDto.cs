using Abp.Application.Services.Dto;

namespace ELog.Application.CommonService.Dispensing.Dto
{
    public class EquipmentCleaningBarcodeDto : EntityDto<int>
    {
        public string EquipmentBarcode { get; set; }
        public int EquipmentId { get; set; }
        public int? AreaId { get; set; }
        public string AreaBarcode { get; set; }
        public int? CubicleId { get; set; }
        public string CubicleBarcode { get; set; }
        public int EquipmentTypeId { get; set; }
        public int? PlantId { get; set; }
        public string Status { get; set; }
    }
}