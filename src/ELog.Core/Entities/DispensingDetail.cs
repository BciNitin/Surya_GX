using ELog.Core.Authorization;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ELog.Core.Entities
{
    [Table("DispensingDetails")]
    public class DispensingDetail : PMMSFullAudit
    {
        [ForeignKey("DispensingHeaderId")]
        public int? DispensingHeaderId { get; set; }

        [Required]
        [StringLength(PMMSConsts.Medium)]
        public string SAPBatchNumber { get; set; }

        [Required]
        [StringLength(PMMSConsts.Medium)]
        public string ContainerMaterialBarcode { get; set; }

        [Required]
        [StringLength(PMMSConsts.Medium)]
        public string DispenseBarcode { get; set; }

        [ForeignKey("UnitOfMeasurementId")]
        public int? UnitOfMeasurementId { get; set; }

        public int? NoOfPacks { get; set; }

        public bool IsGrossWeight { get; set; }
        public float? GrossWeight { get; set; }
        public float? TareWeight { get; set; }
        public float? NetWeight { get; set; }

        [ForeignKey("WeighingMachineId")]
        public int? WeighingMachineId { get; set; }

        [ForeignKey("SamplingTypeId")]
        public int? SamplingTypeId { get; set; }
        public int? DoneBy { get; set; }
        public int? CheckedById { get; set; }

        public bool Printed { get; set; }

        [ForeignKey("DispensingDetailId")]
        public ICollection<DispensingPrintDetail> DispensingPrintDetails { get; set; }

        public int NoOfContainers { get; set; }

        public int ContainerNo { get; set; }


    }
}