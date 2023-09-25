using ELog.Core.Authorization;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ELog.Core.Entities
{
    [Table("EquipmentCleaningTypeMaster")]
    public class EquipmentCleaningTypeMaster : PMMSFullAudit
    {
        [StringLength(PMMSConsts.Medium)]
        [Required]
        public string Value { get; set; }

        [ForeignKey("CleaningTypeId")]
        public ICollection<EquipmentCleaningTransaction> EquipmentCleaningTransactions { get; set; }
    }
}