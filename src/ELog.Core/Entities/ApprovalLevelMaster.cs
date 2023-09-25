using ELog.Core.Authorization;
using System.ComponentModel.DataAnnotations.Schema;

namespace ELog.Core.Entities
{
    [Table("ApprovalLevelMaster")]
    public class ApprovalLevelMaster : PMMSFullAudit
    {

        public int LevelCode { get; set; }
        public string LevelName { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }

    }
}
