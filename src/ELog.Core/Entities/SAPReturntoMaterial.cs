using ELog.Core.Authorization;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ELog.Core.Entities
{
    [Table("SAPReturntoMaterial")]
    public class SAPReturntoMaterial : PMMSFullAudit
    {
        [StringLength(PMMSConsts.Medium)]
        public string MaterialDocumentNo { get; set; }

        [StringLength(PMMSConsts.Medium)]
        public string MaterialDocumentYear { get; set; }

        [Required]
        [StringLength(PMMSConsts.Medium)]
        public string ItemCode { get; set; }

        public string LineItemNo { get; set; }

        [StringLength(PMMSConsts.Medium)]
        public string MaterialDescription { get; set; }

        [StringLength(PMMSConsts.Medium)]
        public string SAPBatchNo { get; set; }

        public decimal Qty { get; set; }

        [StringLength(PMMSConsts.Medium)]
        public string UOM { get; set; }
    }
}