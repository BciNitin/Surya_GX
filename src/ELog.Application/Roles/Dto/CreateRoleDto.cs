
using ELog.Core;
using ELog.Core.Authorization.Roles;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ELog.Application.Roles.Dto
{
    public class CreateRoleDto
    {
        [Required]
        [StringLength(PMMSConsts.Medium)]
        public string Name { get; set; }

        [Required]
        [StringLength(PMMSConsts.Medium)]
        public string DisplayName { get; set; }

        [Required]
        [DefaultValue(false)]
        public bool? IsDeleted { get; set; }

        [Required]
        [DefaultValue(true)]
        public bool? IsActive { get; set; }

        [StringLength(Role.MaxDescriptionLength)]
        public string Description { get; set; }

        [Required]
        public DateTime? CreatedOn { get; set; }

        [Required]
        public List<RolePermissionsDto> ModulePermissions { get; set; }
    }
}