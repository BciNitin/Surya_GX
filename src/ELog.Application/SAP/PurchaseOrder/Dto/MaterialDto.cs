using ELog.Core;

using System.ComponentModel.DataAnnotations;

namespace ELog.Application.SAP.PurchaseOrder.Dto
{
    public class MaterialDto
    {
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
        public string UnitOfMeasurement { get; set; }
    }
}