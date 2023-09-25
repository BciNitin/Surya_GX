using ELog.Core.Authorization;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ELog.Core.Entities
{
    [Table("LineClearanceCheckpoints")]
    public class LineClearanceCheckpoint : PMMSFullAudit
    {
        [ForeignKey("LineClearanceTransactionId")]
        public int LineClearanceTransactionId { get; set; }

        [ForeignKey("CheckPointId")]
        public int CheckPointId { get; set; }

        [StringLength(PMMSConsts.Large)]
        public string Observation { get; set; }

        [StringLength(PMMSConsts.Large)]
        public string Remark { get; set; }
    }
}