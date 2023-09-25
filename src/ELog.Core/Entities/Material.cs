using ELog.Core.Authorization;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ELog.Core.Entities
{
    [Table("Materials")]
    public class Material : PMMSFullAudit
    {
        [ForeignKey("PurchaseOrderId")]
        [Required]
        public int PurchaseOrderId { get; set; }

        [StringLength(PMMSConsts.Medium)]
        [Required]
        public string PurchaseOrderNo { get; set; }

        [Required]
        [StringLength(PMMSConsts.Medium)]
        public string ItemNo { get; set; }

        [Required]
        [StringLength(PMMSConsts.Medium)]
        public string ItemCode { get; set; }

        public string ItemDescription { get; set; }

        public float OrderQuantity { get; set; }

        [StringLength(PMMSConsts.Medium)]
        public string UnitOfMeasurement { get; set; }
        public float? BalanceQuantity { get; set; }
        [StringLength(PMMSConsts.Medium)]
        public string ManufacturerName { get; set; }
        [StringLength(PMMSConsts.Medium)]
        public string ManufacturerCode { get; set; }
        public int? TenantId { get; set; }
        [ForeignKey("MaterialId")]
        public ICollection<MaterialInspectionRelationDetail> MaterialInspectionRelationDetails { get; set; }

        [ForeignKey("MaterialId")]
        public ICollection<GRNDetail> GRNDetails { get; set; }

        [ForeignKey("MaterialId")]
        public ICollection<Palletization> Palletizations { get; set; }

        [ForeignKey("MaterialId")]
        public ICollection<PutAwayBinToBinTransfer> PutAwayBinToBinTransfers { get; set; }
    }
}