using ELog.Core.Authorization;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ELog.Core.Entities
{
    [Table("HandlingUnitTypeMaster")]
    public class HandlingUnitTypeMaster : PMMSFullAudit
    {
        [Required]
        [StringLength(PMMSConsts.Medium)]
        public string HandlingUnitName { get; set; }

        [ForeignKey("HandlingUnitTypeId")]
        public ICollection<HandlingUnitMaster> HandlingUnitMasters { get; set; }
    }
}