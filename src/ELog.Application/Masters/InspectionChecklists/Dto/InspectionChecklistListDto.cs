using Abp.Application.Services.Dto;
using Abp.AutoMapper;

using ELog.Core.Entities;

namespace ELog.Application.Masters.InspectionChecklists.Dto
{
    [AutoMapFrom(typeof(InspectionChecklistMaster))]
    public class InspectionChecklistListDto : EntityDto<int>
    {
        public string ChecklistCode { get; set; }
        public string Name { get; set; }
        public int PlantId { get; set; }
        public int SubModuleId { get; set; }

        public string UserEnteredPlantId { get; set; }
        public string UserEnteredSubModuleId { get; set; }
        public bool IsActive { get; set; }
        public int ApprovalStatusId { get; set; }
        public string UserEnteredApprovalStatus { get; set; }
    }
}