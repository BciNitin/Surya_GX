using ELog.Core.Authorization;
using System.ComponentModel.DataAnnotations.Schema;

namespace ELog.Core.Entities
{
    [Table("FgPicking")]
    public class FgPicking : PMMSFullAudit
    {
        public string OBD { get; set; }
        public int? ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductCode { get; set; }
        public string ProductBatchNo { get; set; }
        public string LineItem { get; set; }
        public string Batch { get; set; }
        public string Description { get; set; }
        public int? SuggestedLocationId { get; set; }
        public int? LocationId { get; set; }
        public string LocationBarcode { get; set; }
        public string PalletBarcode { get; set; }
        public int? PalletCount { get; set; }
        public int? ShipperCount { get; set; }
        public int? Quantity { get; set; }
        public string UOM { get; set; }
        public int? NoOfPacks { get; set; }
        public bool? isActive { get; set; }
        public bool? isPicked { get; set; }
        public string HUCode { get; set; }
        public int? PlantId { get; set; }
    }
}
