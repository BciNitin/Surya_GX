using System.ComponentModel.DataAnnotations.Schema;

namespace ELog.Core.Entities
{
    [Table("ZMaster")]
    public class ZMaster : PMMSFullAuditWithApprovalStatus
    {
        public string ZField { get; set; }
        public string DescriptionField { get; set; }
    }
}