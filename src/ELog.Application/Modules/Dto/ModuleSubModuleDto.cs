using Abp.Application.Services.Dto;
using Abp.AutoMapper;

using ELog.Core.Entities;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ELog.Application.Modules.Dto
{
    [AutoMapFrom(typeof(ModuleSubModule))]
    public class ModuleSubModuleDto : EntityDto<int>
    {
        public List<SubModuleDto> SubModule { get; set; }

        [Required]
        public bool IsMandatory { get; set; }

        [Required]
        public bool IsSelected { get; set; }

        [Required]
        public bool IsActive { get; set; }
    }
}