using Abp.Application.Services.Dto;
using Abp.AutoMapper;

using ELog.Core.Entities;

namespace ELog.Application.Masters.Equipments.Dto
{
    [AutoMapFrom(typeof(EquipmentMaster))]
    public class EquipmentListDto : EntityDto<int>
    {
        public int PlantId { get; set; }
        public string UserEnteredPlantId { get; set; }
        public string UserEnteredEquipment { get; set; }
        public int? SLOCId { get; set; }
        public int? EquipmentTypeId { get; set; }
        public string EquipmentCode { get; set; }
        public bool IsActive { get; set; }
        public int ApprovalStatusId { get; set; }
        public string UserEnteredApprovalStatus { get; set; }
    }
}