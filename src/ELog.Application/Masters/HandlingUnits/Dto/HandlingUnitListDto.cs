using Abp.Application.Services.Dto;
using Abp.AutoMapper;

using ELog.Core.Entities;

namespace ELog.Application.Masters.HandlingUnits.Dto
{
    [AutoMapFrom(typeof(HandlingUnitMaster))]
    public class HandlingUnitListDto : EntityDto<int>
    {
        public int? PlantId { get; set; }
        public string UserEnteredPlantId { get; set; }
        public string HUCode { get; set; }
        public string Name { get; set; }
        public int? HandlingUnitTypeId { get; set; }
        public string UserEnteredHandlingUnit { get; set; }
        public bool IsActive { get; set; }

        public string Description { get; set; }
        public int ApprovalStatusId { get; set; }
        public string UserEnteredApprovalStatus { get; set; }
    }
}