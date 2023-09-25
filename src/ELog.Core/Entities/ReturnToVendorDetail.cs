using ELog.Core.Authorization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ELog.Core.Entities
{
    [Table("ReturnToVendorDetail")]
    public class ReturnToVendorDetail : PMMSFullAudit
    {
        [ForeignKey("ReturnToVendorHeaderId")]
        public int? ReturnToVendorHeaderId { get; set; }

        [Required]
        [StringLength(PMMSConsts.Medium)]
        public string ContainerMaterialBarcode { get; set; }

        [StringLength(PMMSConsts.Medium)]
        public string UOM { get; set; }
        public float? Qty { get; set; }
    }
}
