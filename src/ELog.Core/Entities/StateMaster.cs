using ELog.Core.Authorization;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ELog.Core.Entities
{
    [Table("StateMaster")]
    public class StateMaster : PMMSFullAudit
    {
        [ForeignKey("CountryId")]
        public int CountryId { get; set; }

        [Required]
        [StringLength(PMMSConsts.Medium)]
        public string StateName { get; set; }

        [ForeignKey("StateId")]
        public ICollection<PlantMaster> Plants { get; set; }
    }
}