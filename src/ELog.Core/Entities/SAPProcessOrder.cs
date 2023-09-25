using ELog.Core.Authorization;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ELog.Core.Entities
{
    [Table("SAPProcessOrders")]
    public class SAPProcessOrder : PMMSFullAudit
    {
        [StringLength(PMMSConsts.Medium)]
        [Required]
        public string ProcessOrderNo { get; set; }

        [StringLength(PMMSConsts.Medium)]
        public string LineItemNo { get; set; }

        [StringLength(PMMSConsts.Medium)]
        public string MaterialCode { get; set; }

        public string MaterialDescription { get; set; }

        [StringLength(PMMSConsts.Medium)]
        public string ARNo { get; set; }

        [StringLength(PMMSConsts.Medium)]
        public string SAPBatchNo { get; set; }

        [StringLength(PMMSConsts.Medium)]
        public string ProductCode { get; set; }

        [StringLength(PMMSConsts.Medium)]
        public string ProductBatchNo { get; set; }

        public decimal ReqDispensedQty { get; set; }

        [StringLength(PMMSConsts.Medium)]
        public string UOM { get; set; }

        [StringLength(PMMSConsts.Medium)]
        public string BaseUOM { get; set; }
        public decimal BaseQty { get; set; }
        [StringLength(PMMSConsts.Medium)]
        public string CurrentStage { get; set; }
        [StringLength(PMMSConsts.Medium)]
        public string NextStage { get; set; }
        public decimal DispensingQty { get; set; }
        public string DispensingUOM { get; set; }
        [StringLength(PMMSConsts.Medium)]
        public string Plant { get; set; }
        [StringLength(PMMSConsts.Medium)]
        public string StorageLocation { get; set; }
        [Required]
        public bool IsReservationNo { get; set; }
    }
}