using Abp.Application.Services.Dto;

using ELog.Core;
using ELog.Core.Authorization.Roles;

using System.ComponentModel.DataAnnotations;

namespace ELog.Application.Roles.Dto
{
    public class RoleEditDto : EntityDto<int>
    {
        [Required]
        [StringLength(PMMSConsts.Medium)]
        public string Name { get; set; }

        [Required]
        [StringLength(PMMSConsts.Medium)]
        public string DisplayName { get; set; }

        [StringLength(Role.MaxDescriptionLength)]
        public string Description { get; set; }

        public bool IsStatic { get; set; }
    }
}