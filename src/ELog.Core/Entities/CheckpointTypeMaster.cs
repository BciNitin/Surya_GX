using ELog.Core.Authorization;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ELog.Core.Entities
{
    [Table("CheckpointTypeMaster")]
    public class CheckpointTypeMaster : PMMSFullAudit
    {
        [StringLength(PMMSConsts.Small)]
        public string Title { get; set; }
    }
}