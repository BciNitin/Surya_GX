using ELog.Core.Authorization;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ELog.Core.Entities
{
    [Table("MaterialChecklistDetails")]
    public class MaterialChecklistDetail : PMMSFullAudit
    {
        [ForeignKey("CheckPointId")]
        public int? CheckPointId { get; set; }

        [StringLength(PMMSConsts.Large)]
        public string Observation { get; set; }

        [StringLength(PMMSConsts.Large)]
        public string DiscrepancyRemark { get; set; }

        [ForeignKey("MaterialRelationId")]
        public int? MaterialRelationId { get; set; }
    }
}