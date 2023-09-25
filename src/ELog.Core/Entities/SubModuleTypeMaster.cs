using ELog.Core.Authorization;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ELog.Core.Entities
{
    [Table("SubModuleTypeMaster")]
    public class SubModuleTypeMaster : PMMSFullAudit
    {
        [StringLength(PMMSConsts.Medium)]
        public string SubModuleType { get; set; }

        [ForeignKey("SubModuleTypeId")]
        public ICollection<SubModuleMaster> SubModules { get; set; }
    }
}