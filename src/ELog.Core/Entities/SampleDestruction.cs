using ELog.Core.Authorization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ELog.Core.Entities
{
    [Table("SampleDestructions")]
    public class SampleDestruction : PMMSFullAudit
    {
        [Required]
        [ForeignKey("InspectionLotId")]
        public int? InspectionLotId { get; set; }

        [Required]
        [StringLength(PMMSConsts.Medium)]
        public string MaterialCode { get; set; }

        [Required]
        [StringLength(PMMSConsts.Medium)]
        public string SAPBatchNumber { get; set; }

        [ForeignKey("UnitOfMeasurementId")]
        public int? UnitOfMeasurementId { get; set; }

        [Required]
        [StringLength(PMMSConsts.Medium)]
        public string ContainerMaterialBarcode { get; set; }
        public int? NoOfPacks { get; set; }

        public float? GrossWeight { get; set; }
        public float? TareWeight { get; set; }
        public float? NetWeight { get; set; }

        [ForeignKey("WeighingMachineId")]
        public int? WeighingMachineId { get; set; }
        public int? TenantId { get; set; }
    }
}