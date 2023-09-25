using ELog.Core.Authorization;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ELog.Core.Entities
{
    [Table("CubicleCleaningCheckpoints")]
    public class CubicleCleaningCheckpoint : PMMSFullAudit
    {
        [ForeignKey("CheckPointId")]
        public int CheckPointId { get; set; }

        [StringLength(PMMSConsts.Large)]
        public string Observation { get; set; }

        [StringLength(PMMSConsts.Large)]
        public string Remark { get; set; }

        [ForeignKey("CubicleCleaningTransactionId")]
        public int CubicleCleaningTransactionId { get; set; }
    }
}
