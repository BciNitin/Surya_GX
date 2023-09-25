using Abp.Application.Services.Dto;
using Abp.AutoMapper;

using ELog.Core;
using ELog.Core.Entities;

using System.ComponentModel.DataAnnotations;

namespace ELog.Application.Modules.Dto
{
    [AutoMapFrom(typeof(SubModuleMaster))]
    public class UpdateSubModuleDto : EntityDto<int>
    {
        [Required]
        [StringLength(PMMSConsts.Medium)]
        public string Name { get; set; }

        [Required]
        [StringLength(PMMSConsts.Medium)]
        public string DisplayName { get; set; }

        [Required]
        public string Description { get; set; }

        public bool IsSelected { get; set; }

        public bool IsMandatory { get; set; }

        public bool IsDeleted { get; set; }

        public bool IsActive { get; set; }
        public bool IsApprovalRequired { get; set; }
    }
}