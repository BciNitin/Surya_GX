using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ELog.Core.Entities
{
    [Table("InspectionChecklistMaster")]
    public class InspectionChecklistMaster : PMMSFullAuditWithApprovalStatus
    {
        [Required]
        [StringLength(PMMSConsts.Small)]
        public string ChecklistCode { get; set; }

        [Required]
        [StringLength(PMMSConsts.Medium)]
        public string Name { get; set; }

        [ForeignKey("PlantMaster")]
        public int PlantId { get; set; }

        [ForeignKey("SubModuleMaster")]
        public int SubModuleId { get; set; }

        public bool IsActive { get; set; }

        public string Description { get; set; }

        public int Version { get; set; }

        [StringLength(PMMSConsts.Medium)]
        public string FormatNumber { get; set; }

        [Required]
        [ForeignKey("ChecklistTypeId")]
        public int ChecklistTypeId { get; set; }

        [Required]
        [ForeignKey("ModeId")]
        public int ModeId { get; set; }

        public int? TenantId { get; set; }

        [ForeignKey("InspectionChecklistId")]
        public ICollection<CheckpointMaster> CheckpointMasters { get; set; }

        [ForeignKey("InspectionChecklistId")]
        public ICollection<VehicleInspectionHeader> VehicleInspectionHeader { get; set; }

        [ForeignKey("InspectionChecklistId")]
        public ICollection<MaterialInspectionRelationDetail> MaterialInspectionRelationDetails { get; set; }

        [ForeignKey("InspectionChecklistId")]
        public ICollection<WMCalibrationHeader> WMCalibrationHeaders { get; set; }
    }
}