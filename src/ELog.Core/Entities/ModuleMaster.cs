using ELog.Core.Authorization;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ELog.Core.Entities
{
    [Table("ModuleMaster")]
    public class ModuleMaster : PMMSFullAudit
    {
        [Required]
        [StringLength(PMMSConsts.Medium)]
        public string Name { get; set; }

        [Required]
        [StringLength(PMMSConsts.Medium)]
        public string DisplayName { get; set; }

        [Required]
        public string Description { get; set; }

        public int? TenantId { get; set; }

        public bool IsActive { get; set; }

        [ForeignKey("ModuleId")]
        public ICollection<ModuleSubModule> ModuleSubModules { get; set; }

        [ForeignKey("ModuleId")]
        public ICollection<StatusMaster> StatusMasters { get; set; }
    }
}