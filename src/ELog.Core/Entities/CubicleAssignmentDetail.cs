using ELog.Core.Authorization;

using System.ComponentModel.DataAnnotations.Schema;

namespace ELog.Core.Entities
{
    [Table("CubicleAssignmentDetails")]
    public class CubicleAssignmentDetail : PMMSFullAudit
    {
        [ForeignKey("CubicleAssignmentHeaderId")]
        public int? CubicleAssignmentHeaderId { get; set; }

        public int? TenantId { get; set; }

        [ForeignKey("ProcessOrderId")]
        public int? ProcessOrderId { get; set; }

        [ForeignKey("ProcessOrderMaterialId")]
        public int? ProcessOrderMaterialId { get; set; }

        [ForeignKey("CubicleId")]
        public int? CubicleId { get; set; }

        [ForeignKey("StatusId")]
        public int? StatusId { get; set; }

        [ForeignKey("InspectionLotId")]
        public int? InspectionLotId { get; set; }
    }
}