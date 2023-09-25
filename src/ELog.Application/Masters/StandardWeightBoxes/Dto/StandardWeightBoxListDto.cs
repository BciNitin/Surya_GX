using Abp.Application.Services.Dto;
using Abp.AutoMapper;

using ELog.Core.Entities;

namespace ELog.Application.Masters.StandardWeightBoxes.Dto
{
    [AutoMapFrom(typeof(StandardWeightBoxMaster))]
    public class StandardWeightBoxListDto : EntityDto<int>
    {
        public int SubPlantId { get; set; }
        public string StandardWeightBoxId { get; set; }

        public int? AreaId { get; set; }
        public int DepartmentId { get; set; }

        public string UserEnteredSubPlantId { get; set; }

        public string UserEnteredDepartmentId { get; set; }

        public string UserEnteredAreaId { get; set; }
        public bool IsActive { get; set; }
        public string Description { get; set; }
        public int ApprovalStatusId { get; set; }
        public string UserEnteredApprovalStatus { get; set; }
    }
}