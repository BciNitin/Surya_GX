using ELog.Core.Authorization;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ELog.Core.Entities
{
    [Table("CubicleCleaningTypeMaster")]
    public class CubicleCleaningTypeMaster : PMMSFullAudit
    {
        [StringLength(PMMSConsts.Medium)]
        [Required]
        public string Value { get; set; }

        [ForeignKey("TypeId")]
        public ICollection<CubicleCleaningTransaction> CubicleCleaningTransactions { get; set; }
    }
}