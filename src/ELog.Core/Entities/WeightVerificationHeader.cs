using ELog.Core.Authorization;
using System.ComponentModel.DataAnnotations.Schema;

namespace ELog.Core.Entities
{
    [Table("WeightVerificationHeader")]
    public class WeightVerificationHeader : PMMSFullAudit
    {
        public int? ProductId { get; set; }
        public string ProductCode { get; set; }
        public int? BatchId { get; set; }
        public int? LotId { get; set; }
        public int? ProcessOrderId { get; set; }
        public int? CubicalId { get; set; }
        public int? DispensedId { get; set; }
        public int? UnitofMeasurementId { get; set; }

        public int? NoOfContainers { get; set; }
        public int? NoOfPacks { get; set; }
        public int? RecivedNoOfPacks { get; set; }
        public int ScanBalanceId { get; set; }
        public string ScanBalanceNo { get; set; }
        public float? DispGrossWeight { get; set; }
        public float? NetWeight { get; set; }
        public float? TareWeight { get; set; }
        public float? GrossWeight { get; set; }
        public bool IsGrossWeight { get; set; }

    }
}
