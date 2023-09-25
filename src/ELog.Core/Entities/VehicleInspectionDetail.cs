using ELog.Core.Authorization;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ELog.Core.Entities
{
    [Table("VehicleInspectionDetails")]
    public class VehicleInspectionDetail : PMMSFullAudit
    {
        [ForeignKey("VehicleInspectionHeaderId")]
        public int? VehicleInspectionHeaderId { get; set; }

        public int? TenantId { get; set; }

        [ForeignKey("CheckpointId")]
        public int? CheckpointId { get; set; }

        [Required]
        [StringLength(PMMSConsts.Large)]
        public string Observation { get; set; }

        [StringLength(PMMSConsts.Large)]
        public string DiscrepancyRemark { get; set; }
    }
}