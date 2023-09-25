using Abp.AutoMapper;

using ELog.Core;
using ELog.Core.Entities;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ELog.Application.Modules.Dto
{
    [AutoMapTo(typeof(ModuleMaster))]
    public class CreateModuleDto
    {
        [Required]
        [StringLength(PMMSConsts.Medium)]
        public string Name { get; set; }

        [Required]
        [StringLength(PMMSConsts.Medium)]
        public string DisplayName { get; set; }

        [Required]
        public string Description { get; set; }

        [DefaultValue(false)]
        public bool IsDeleted { get; set; }

        [DefaultValue(true)]
        public bool IsActive { get; set; }

        [Required]
        public DateTime CreatedOn { get; set; }

        public int? TenantId { get; set; }

        public List<SubModuleDto> SubModules { get; set; }
    }
}