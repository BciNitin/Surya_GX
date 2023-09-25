using Abp.AutoMapper;
using ELog.Core.Entities;
using System.ComponentModel.DataAnnotations;

namespace ELog.Application.TransactionApprovalMatrix.ApprovalUserModuleMapping.Dto
{
    [AutoMapTo(typeof(ApprovalUserModuleMappingMaster))]
    public class CreateApprovalUserModuleMappingDto
    {
        [Required(ErrorMessage = "Approval Level is required.")]
        public int AppLevelId { get; set; }

        [Required(ErrorMessage = "User is required.")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Module is required.")]
        public int ModuleId { get; set; }

        [Required(ErrorMessage = "SubModule is required.")]
        public int SubModuleId { get; set; }
        public bool IsActive { get; set; }
    }
}
