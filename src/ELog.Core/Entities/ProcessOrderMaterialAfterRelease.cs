using ELog.Core.Authorization;

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ELog.Core.Entities
{
    [Table("ProcessOrderMaterialAfterRelease")]
    public class ProcessOrderMaterialAfterRelease : PMMSFullAudit
    {
        [ForeignKey("ProcessOrderAfterReleaseId")]
        public int? ProcessOrderId { get; set; }

        [StringLength(PMMSConsts.Medium)]
        public string ProcessOrderNo { get; set; }
        public string LineItemNo { get; set; }

        public string MaterialCode { get; set; }
        public string MaterialDescription { get; set; }
        public string ARNO { get; set; }

        public string LotNo { get; set; }

        public string SAPBatchNo { get; set; }
        public string ProductBatchNo { get; set; }
        public string CurrentStage { get; set; }
        public string NextStage { get; set; }

        public float Quantity { get; set; }
        public string UOM { get; set; }

        public DateTime ExpiryDate { get; set; }

        public DateTime RetestDate { get; set; }
    }
}
