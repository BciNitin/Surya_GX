using ELog.Core.Authorization;
using System.ComponentModel.DataAnnotations.Schema;

namespace ELog.Core.Entities
{


    [Table("FgPutAway")]
    public class FgPutAway : PMMSFullAudit
    {
        public int? PalletId { get; set; }
        public string PalletBarcode { get; set; }
        public int PalletCount { get; set; }
        public int? LocationId { get; set; }
        public string LocationBarcode { get; set; }
        public bool isActive { get; set; }
        public bool isPicked { get; set; }
        public string HUCode { get; set; }
        public int? PlantId { get; set; }
        public string ProductBatchNo { get; set; }
    }
}

