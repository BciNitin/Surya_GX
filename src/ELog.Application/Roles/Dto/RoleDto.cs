using Abp.Application.Services.Dto;

using ELog.Core;
using ELog.Core.Authorization.Roles;

using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ELog.Application.Roles.Dto
{
    public class RoleDto : EntityDto<int>
    {
        [Required]
        [StringLength(PMMSConsts.Medium)]
        public string Name { get; set; }

        [Required]
        [StringLength(PMMSConsts.Medium)]
        public string DisplayName { get; set; }

        [StringLength(Role.MaxDescriptionLength)]
        public string Description { get; set; }

        [Required]
        [DefaultValue(false)]
        public bool IsDeleted { get; set; }

        [Required]
        [DefaultValue(true)]
        public bool IsActive { get; set; }

        [Required]
        public List<RolePermissionsDto> ModulePermissions { get; set; }

        public bool IsApprovalRequired { get; set; }
        public string ApprovalStatusDescription { get; set; }
        public string UserEnteredApprovalStatus { get; set; }
        public bool isSuperAdminRole { get; set; }
    }
}