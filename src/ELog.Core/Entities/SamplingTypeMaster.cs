using ELog.Core.Authorization;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ELog.Core.Entities
{
    [Table("SamplingTypeMaster")]
    public class SamplingTypeMaster : PMMSFullAudit
    {
        [StringLength(PMMSConsts.Medium)]
        public string Type { get; set; }

        [ForeignKey("SamplingTypeId")]
        public ICollection<DispensingDetail> DispensingDetails { get; set; }
    }
}