using ELog.Core;

using System.ComponentModel.DataAnnotations;

namespace ELog.Application.SAP.Dto
{
    public class SAPReturntoMaterialDto
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