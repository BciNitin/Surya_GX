using Abp.Application.Services.Dto;
using Abp.AutoMapper;

using ELog.Core.Entities;

namespace ELog.Application.Masters.Areas.Dto
{
    [AutoMapFrom(typeof(AreaMaster))]
    public class AreaListDto : EntityDto<int>
    {
        public int SubPlantId { get; set; }
        public int DepartmentId { get; set; }

        public string AreaCode { get; set; }
        public string AreaName { get; set; }
        public string UserEnteredSubPlantId { get; set; }
        public string UserEnteredDepartmentId { get; set; }

        public bool IsActive { get; set; }
        public string Description { get; set; }
        public int ApprovalStatusId { get; set; }
        public string UserEnteredApprovalStatus { get; set; }

    }
}