using System.ComponentModel.DataAnnotations.Schema;
namespace ELog.Core.Entities
{
    [Table("AtesttableMaster")]
    public class AtesttableMaster : PMMSFullAuditWithApprovalStatus
    {
        public string testfield1 { get; set; }
        public string testfield2 { get; set; }
        public string testfield3 { get; set; }
        public string testfield4 { get; set; }
    }
}
