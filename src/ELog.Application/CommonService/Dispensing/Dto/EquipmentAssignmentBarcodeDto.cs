using System;

namespace ELog.Application.CommonService.Dispensing.Dto
{
    public class EquipmentAssignmentBarcodeDto
    {
        public string EquipmentBarcode { get; set; }
        public int EquipmentId { get; set; }

        public string EquipmentName { get; set; }

        public int? PlantId { get; set; }
        public DateTime? LastCleaningDate { get; set; }
        public int CleanHoldTime { get; set; }
        public bool IsSampling { get; set; }
        public string EquipmentType { get; set; }
        public string Status { get; set; }
    }
}