using ELog.Core.Authorization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ELog.Core.Entities
{
    [Table("StageOutDetails")]
    public class StageOutDetail : PMMSFullAudit
    {
        [ForeignKey("StageOutHeaderId")]
        public int StageOutHeaderId { get; set; }

        [StringLength(PMMSConsts.Medium)]
        public string SAPBatchNo { get; set; }

        [StringLength(PMMSConsts.Medium)]
        public string MaterialContainerBarcode { get; set; }

        public float BalanceQuantity { get; set; }
    }
}