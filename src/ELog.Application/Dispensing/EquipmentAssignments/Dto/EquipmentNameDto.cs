using Abp.Application.Services.Dto;

namespace ELog.Application.Dispensing.EquipmentAssignments.Dto
{
    public class EquipmentNameDto : EntityDto<int>
    {
        public int EquipmentId { get; set; }
        public string EquipmentBarCode { get; set; }
        public string EquipmentName { get; set; }
        public bool IsAssignedOrDeAssigned { get; set; }
        public bool IsSampling { get; set; }
        public string EquipmentType { get; set; }
    }
}