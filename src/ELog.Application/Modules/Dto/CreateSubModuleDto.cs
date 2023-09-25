using Abp.AutoMapper;

using ELog.Core;
using ELog.Core.Entities;

using System;
using System.ComponentModel.DataAnnotations;

namespace ELog.Application.Modules.Dto
{
    [AutoMapTo(typeof(SubModuleMaster))]
    public class CreateSubModuleDto
    {
        [Required]
        [StringLength(PMMSConsts.Medium)]
        public string Name { get; set; }

        [Required]
        [StringLength(PMMSConsts.Medium)]
        public string DisplayName { get; set; }

        [Required]
        public string Description { get; set; }

        public bool IsDeleted { get; set; }

        public bool IsActive { get; set; }

        [Required]
        public DateTime CreatedOn { get; set; }

        public int? TenantId { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Sequence must be grater than zero.")]
        public int Sequence { get; set; }

        [Required]
        public int? SubModuleTypeId { get; set; }

        public bool IsApprovalRequired { get; set; }
    }
}