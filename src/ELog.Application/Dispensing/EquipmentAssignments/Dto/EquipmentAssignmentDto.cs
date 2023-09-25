using System.Collections.Generic;

namespace ELog.Application.Dispensing.EquipmentAssignments.Dto
{
    public class EquipmentAssignmentDto
    {
        public string CubicleCode { get; set; }
        public int? CubicleId { get; set; }
        public string GroupId { get; set; }
        public string ProductCode { get; set; }
        public string ProductDescription { get; set; }
        public string BatchNumber { get; set; }
        public int PlantId { get; set; }
        public int? CubicleAssignmentHeaderId { get; set; }
        public List<EquipmentNameDto> lstEquipments { get; set; }
        public List<EquipmentNameDto> FixedEquipments { get; set; }
        public string SAPBatchNo { get; set; }
        public bool IsReservationNo { get; set; }

    }

    public class EquipmentBarcodeRequestDto
    {
        public int CubicleId { get; set; }
        public string EquipmentBarcode { get; set; }
        public string GroupId { get; set; }
    }
}