using ELog.Core.Authorization;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ELog.Core.Entities
{
    [Table("EquipmentTypeMaster")]
    public class EquipmentTypeMaster : PMMSFullAudit
    {
        [Required]
        [StringLength(PMMSConsts.Medium)]
        public string EquipmentName { get; set; }

        [ForeignKey("EquipmentTypeId")]
        public ICollection<EquipmentMaster> EquipmentMasters { get; set; }
    }
}