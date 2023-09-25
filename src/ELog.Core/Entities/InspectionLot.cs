using ELog.Core.Authorization;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ELog.Core.Entities
{
    [Table("InspectionLot")]
    public class InspectionLot : PMMSFullAudit
    {
        [ForeignKey("PlantId")]
        [Required]
        public int PlantId { get; set; }

        [Required]
        [StringLength(PMMSConsts.Medium)]
        public string InspectionLotNumber { get; set; }

        [StringLength(PMMSConsts.Medium)]
        public string ProductCode { get; set; }

        public int? TenantId { get; set; }

        [ForeignKey("InspectionLotId")]
        public ICollection<ProcessOrderMaterial> ProcessOrderMaterials { get; set; }

        [ForeignKey("InspectionLotId")]
        public ICollection<CubicleAssignmentDetail> CubicleAssignmentDetails { get; set; }

        [ForeignKey("InspectionLotId")]
        public ICollection<DispensingHeader> DispensingHeaders { get; set; }
    }
}