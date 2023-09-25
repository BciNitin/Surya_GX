using ELog.Core.Authorization;
using System.ComponentModel.DataAnnotations.Schema;

namespace ELog.Core.Entities
{
    [Table("PostWIPDataToSAP")]
    public class PostWIPDataToSAP : PMMSFullAudit
    {
        public int? ProductId { get; set; }
        public int? ProcessOrderId { get; set; }

        public int? InProcessLabelId { get; set; }

        public bool IsActive { get; set; }
        public bool IsSent { get; set; }

    }
}
