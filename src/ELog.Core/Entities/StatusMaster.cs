using ELog.Core.Authorization;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ELog.Core.Entities
{
    [Table("StatusMaster")]
    public class StatusMaster : PMMSFullAudit
    {
        [StringLength(PMMSConsts.Medium)]
        public string Status { get; set; }

        [ForeignKey("ModuleId")]
        public int ModuleId { get; set; }

        [ForeignKey("SubModuleId")]
        public int SubModuleId { get; set; }

    }
}