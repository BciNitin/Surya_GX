using System.ComponentModel.DataAnnotations.Schema;

namespace ELog.Core.Entities
{
    [Table("PRNEntryMaster")]
    public class PRNEntryMaster : PMMSFullAuditWithApprovalStatus
    {
        public string PRNFileName { get; set; }

        [ForeignKey("PlantMaster")]
        public int PlantId { get; set; }
        public int? TenantId { get; set; }

        [ForeignKey("ModuleMaster")]
        public int ModuleId { get; set; }

        [ForeignKey("SubModuleMaster")]
        public int SubModuleId { get; set; }
        public bool IsActive { get; set; }
    }
}
