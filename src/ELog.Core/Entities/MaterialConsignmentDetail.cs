using ELog.Core.Authorization;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ELog.Core.Entities
{
    [Table("MaterialConsignmentDetails")]
    public class MaterialConsignmentDetail : PMMSFullAudit
    {
        [StringLength(PMMSConsts.Medium)]
        public string ManufacturedBatchNo { get; set; }

        public DateTime? ManufacturedDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public DateTime? RetestDate { get; set; }
        public float? QtyAsPerInvoice { get; set; }

        public string QtyAsPerInvoiceInDecimal { get; set; }
        public int? UnitofMeasurementId { get; set; }
        public int? SequenceId { get; set; }
        [ForeignKey("MaterialRelationId")]
        public int? MaterialRelationId { get; set; }
        [ForeignKey("MfgBatchNoId")]
        public ICollection<GRNDetail> GRNDetails { get; set; }
        [ForeignKey("MfgBatchNoId")]
        public ICollection<WeightCaptureHeader> WeightCapture { get; set; }
        [ForeignKey("MaterialConsignmentId")]
        public ICollection<MaterialDamageDetail> MaterialDamageDetails { get; set; }
    }
}