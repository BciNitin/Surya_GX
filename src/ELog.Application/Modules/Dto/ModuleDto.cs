using Abp.Application.Services.Dto;
using Abp.AutoMapper;

using ELog.Core;
using ELog.Core.Entities;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ELog.Application.Modules.Dto
{
    [AutoMap(typeof(ModuleMaster))]
    public class ModuleDto : EntityDto<int>
    {
        [Required]
        [ReadOnly(true)]
        [StringLength(PMMSConsts.Medium)]
        public string Name { get; set; }

        [Required]
        [StringLength(PMMSConsts.Medium)]
        public string DisplayName { get; set; }

        [Required]
        public string Description { get; set; }

        public bool IsDeleted { get; set; }

        public bool IsActive { get; set; }

        public DateTime? CreatedOn { get; set; }

        [Required]
        public List<SubModuleDto> SubModules { get; set; }
    }
}