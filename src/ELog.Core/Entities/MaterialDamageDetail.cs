using ELog.Core.Authorization;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ELog.Core.Entities
{
    [Table("MaterialDamageDetails")]
    public class MaterialDamageDetail : PMMSFullAudit
    {
        public int? SequenceId { get; set; }

        [StringLength(PMMSConsts.Medium)]
        public string ContainerNo { get; set; }

        [StringLength(PMMSConsts.Medium)]
        public string Remark { get; set; }

        public float? Quantity { get; set; }

        public string QuantityInDecimal { get; set; }
        public int? UnitofMeasurementId { get; set; }

        [ForeignKey("MaterialRelationId")]
        public int? MaterialRelationId { get; set; }
        [ForeignKey("MaterialConsignmentId")]
        public int? MaterialConsignmentId { get; set; }
    }
}