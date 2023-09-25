using ELog.Core.Authorization;
using System.ComponentModel.DataAnnotations.Schema;

namespace ELog.Core.Entities
{
    [Table("WeightVerificationDetail")]
    public class WeightVerificationDetail : PMMSFullAudit
    {
        [ForeignKey("WeightVerificationHeaderId")]
        public int WeightVerificationHeaderId { get; set; }

        public int? NoOfContainers { get; set; }
        public int? NoOfPacks { get; set; }
        public int? RecivedNoOfPacks { get; set; }
        public int ScanBalanceId { get; set; }
        public float? GrossWeight { get; set; }
        public float? NetWeight { get; set; }
        public float? TareWeight { get; set; }
    }
}
