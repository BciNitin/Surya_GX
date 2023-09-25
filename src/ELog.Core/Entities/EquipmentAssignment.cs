using ELog.Core.Authorization;
using System.ComponentModel.DataAnnotations.Schema;

namespace ELog.Core.Entities
{
    [Table("EquipmentAssignments")]
    public class EquipmentAssignment : PMMSFullAudit
    {
        [ForeignKey("EquipmentId")]
        public int? EquipmentId { get; set; }

        [ForeignKey("Cubicleid")]
        public int? Cubicleid { get; set; }

        [ForeignKey("CubicleAssignmentHeaderId")]
        public int? CubicleAssignmentHeaderId { get; set; }

        public string GroupId { get; set; }
        public bool IsSampling { get; set; }
    }
}