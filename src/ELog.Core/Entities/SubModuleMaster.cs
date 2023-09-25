using ELog.Core.Authorization;

using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ELog.Core.Entities
{
    [Table("SubModuleMaster")]
    public class SubModuleMaster : PMMSFullAudit
    {
        [Required]
        [StringLength(PMMSConsts.Medium)]
        public string Name { get; set; }

        [Required]
        [StringLength(PMMSConsts.Medium)]
        public string DisplayName { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Sequence must be grater than zero.")]
        public int Sequence { get; set; }

        public int? TenantId { get; set; }

        public bool IsActive { get; set; }

        public int? SubModuleTypeId { get; set; }

        [DefaultValue(false)]
        public bool IsApprovalRequired { get; set; }

        [DefaultValue(false)]
        public bool IsApprovalWorkflowRequired { get; set; }

        [ForeignKey("SubModuleId")]
        public virtual ICollection<ModuleSubModule> ModuleSubModules { get; set; }

        [ForeignKey("SubModuleId")]
        public virtual ICollection<ChecklistTypeMaster> ChecklistTypeMasters { get; set; }

        [ForeignKey("SubModuleId")]
        public ICollection<StatusMaster> StatusMasters { get; set; }

        [ForeignKey("SubModuleId")]
        public ICollection<ReportConfiguration> ReportConfigurations { get; set; }
    }
}