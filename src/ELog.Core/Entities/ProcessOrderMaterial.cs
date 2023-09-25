using ELog.Core.Authorization;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ELog.Core.Entities
{
    [Table("ProcessOrderMaterials")]
    public class ProcessOrderMaterial : PMMSFullAudit
    {
        [ForeignKey("ProcessOrderId")]
        public int? ProcessOrderId { get; set; }

        [StringLength(PMMSConsts.Medium)]
        public string ProcessOrderNo { get; set; }

        [Required]
        [StringLength(PMMSConsts.Medium)]
        public string ItemNo { get; set; }

        [Required]
        [StringLength(PMMSConsts.Medium)]
        public string ItemCode { get; set; }

        public string ItemDescription { get; set; }

        public float? OrderQuantity { get; set; }

        [ForeignKey("UnitOfMeasurementId")]
        public int? UnitOfMeasurementId { get; set; }

        public string UnitOfMeasurement { get; set; }

        public int? TenantId { get; set; }

        [StringLength(PMMSConsts.Medium)]
        public string BatchNo { get; set; }

        [StringLength(PMMSConsts.Medium)]
        public string SAPBatchNo { get; set; }

        public DateTime ExpiryDate { get; set; }

        public DateTime RetestDate { get; set; }

        [StringLength(PMMSConsts.Medium)]
        public string ARNo { get; set; }

        [ForeignKey("InspectionLotId")]
        public int? InspectionLotId { get; set; }

        [StringLength(PMMSConsts.Medium)]
        public string InspectionLotNo { get; set; }

        [ForeignKey("ProcessOrderMaterialId")]
        public ICollection<CubicleAssignmentDetail> CubicleAssignmentDetails { get; set; }
    }
}