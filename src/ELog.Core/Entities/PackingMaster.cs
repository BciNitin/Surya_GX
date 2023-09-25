using ELog.Core.Authorization;
using System.ComponentModel.DataAnnotations.Schema;

namespace ELog.Core.Entities
{
    [Table("PackingMaster")]
    public class PackingMaster : PMMSFullAudit
    {
        public string ProductId { get; set; }

        [ForeignKey("ProcessOrderAfterRelease")]
        public int? ProcessOrderId { get; set; }

        //public string BatchNo { get; set; }

        [ForeignKey("ContainerId")]
        public int? ContainerId { get; set; }
        public int? ContainerCount { get; set; }
        public int? Quantity { get; set; }

        public bool IsActive { get; set; }
    }
}
