using Abp.Application.Services.Dto;

namespace ELog.Application.Masters.Equipments.Dto
{
    public class PagedEquipmentResultRequestDto : PagedAndSortedResultRequestDto
    {
        public int? PlantId { get; set; }
        public int? SLOCId { get; set; }
        public int? EquipmentTypeId { get; set; }
        public string EquipmentCode { get; set; }
        public int? ActiveInactiveStatusId { get; set; }
        public int? ApprovalStatusId { get; set; }
    }
}