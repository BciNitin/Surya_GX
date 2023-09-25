using ELog.Core.Authorization;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ELog.Core.Entities
{
    [Table("CountryMaster")]
    public class CountryMaster : PMMSFullAudit
    {
        [Required]
        [StringLength(PMMSConsts.Medium)]
        public string CountryName { get; set; }

        [ForeignKey("CountryId")]
        public ICollection<StateMaster> States { get; set; }

        [ForeignKey("CountryId")]
        public ICollection<PlantMaster> Plants { get; set; }
    }
}