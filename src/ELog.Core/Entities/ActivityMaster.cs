using System.ComponentModel.DataAnnotations.Schema;
namespace ELog.Core.Entities
{
    [Table("ActivityMaster")]
    public class ActivityMaster : PMMSFullAuditWithApprovalStatus
    {

        public string ActivityName { get; set; }

        public string ActivityCode { get; set; }

        public string Description { get; set; }

        [ForeignKey("ModuleMaster")]
        public int ModuleId { get; set; }

        [ForeignKey("SubModuleMaster")]
        public int SubModuleId { get; set; }

        public int? TenantId { get; set; }
        public bool IsActive { get; set; }
    }
}
