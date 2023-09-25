using Abp.Application.Services.Dto;
using Abp.AutoMapper;

using ELog.Core.Entities;

namespace ELog.Application.Masters.Departments.Dto
{
    [AutoMapFrom(typeof(DepartmentMaster))]
    public class DepartmentListDto : EntityDto<int>
    {
        public int SubPlantId { get; set; }
        public string DepartmentCode { get; set; }

        public string DepartmentName { get; set; }
        public string UserEnteredSubPlantId { get; set; }

        public string UserEnteredDepartmentId { get; set; }

        public bool IsActive { get; set; }
        public string Description { get; set; }
        public int ApprovalStatusId { get; set; }
        public string UserEnteredApprovalStatus { get; set; }

    }
}