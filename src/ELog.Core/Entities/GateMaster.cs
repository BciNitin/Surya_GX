
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ELog.Core.Entities
{
    [Table("GateMaster")]
    public class GateMaster : PMMSFullAuditWithApprovalStatus
    {
        [ForeignKey("PlantMaster")]
        public int PlantId { get; set; }

        [StringLength(PMMSConsts.Small)]
        public string GateCode { get; set; }

        [StringLength(PMMSConsts.Medium)]
        public string Name { get; set; }

        [StringLength(PMMSConsts.Medium)]
        public string AliasName { get; set; }

        public bool IsActive { get; set; }
        public string Description { get; set; }
        public int? TenantId { get; set; }
    }
}