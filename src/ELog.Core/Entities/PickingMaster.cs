using ELog.Core.Authorization;
using System.ComponentModel.DataAnnotations.Schema;

namespace ELog.Core.Entities
{
    [Table("PickingMaster")]
    public class PickingMaster : PMMSFullAudit
    {
        public int? ProcessOrderId { get; set; }
        public int? ProductId { get; set; }
        public string Stage { get; set; }
        public int? SuggestedLocationId { get; set; }
        public int? LocationId { get; set; }
        public int? ContainerId { get; set; }

        public string ContainerCode { get; set; }
        public int? ContainerCount { get; set; }
        public float Quantity { get; set; }
    }
}
