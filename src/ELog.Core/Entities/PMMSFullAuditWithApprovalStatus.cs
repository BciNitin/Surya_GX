using ELog.Core.Authorization;

namespace ELog.Core.Entities
{
    public class PMMSFullAuditWithApprovalStatus : PMMSFullAudit
    {
        public int ApprovalStatusId { get; set; }
        public string ApprovalStatusDescription { get; set; }
    }
}