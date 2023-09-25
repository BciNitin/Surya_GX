using ELog.Core.Authorization;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ELog.Core.Entities
{
    [Table("WeightCaptureDetails")]
    public class WeightCaptureDetail : PMMSFullAudit
    {
        [ForeignKey("WeightCaptureHeaderId")]
        public int WeightCaptureHeaderId { get; set; }

        public int? TenantId { get; set; }

        [ForeignKey("WeighingMachineId")]
        public int ScanBalanceId { get; set; }

        public float? GrossWeight { get; set; }
        public float? NetWeight { get; set; }
        public float? TareWeight { get; set; }
        public int? NoOfPacks { get; set; }
        [StringLength(PMMSConsts.Medium)]
        public string ContainerNo { get; set; }
    }
}