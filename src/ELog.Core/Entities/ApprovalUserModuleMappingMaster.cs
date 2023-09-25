using ELog.Core.Authorization;
using System.ComponentModel.DataAnnotations.Schema;

namespace ELog.Core.Entities
{
    [Table("ApprovalUserModuleMappingMaster")]
    public class ApprovalUserModuleMappingMaster : PMMSFullAudit
    {
        [ForeignKey("ApprovalLevelMaster")]
        public int AppLevelId { get; set; }

        [ForeignKey("Users")]
        public int UserId { get; set; }

        [ForeignKey("ModuleMaster")]
        public int ModuleId { get; set; }

        [ForeignKey("SubModuleMaster")]
        public int SubModuleId { get; set; }
        public bool IsActive { get; set; }
    }
}
