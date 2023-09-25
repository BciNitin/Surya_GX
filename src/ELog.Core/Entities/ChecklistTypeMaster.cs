using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ELog.Core.Entities
{
    [Table("ChecklistTypeMaster")]
    public class ChecklistTypeMaster : PMMSFullAuditWithApprovalStatus
    {
        [StringLength(PMMSConsts.Small)]
        public string ChecklistTypeCode { get; set; }

        [StringLength(PMMSConsts.Medium)]
        public string ChecklistName { get; set; }

        [ForeignKey("SubPlantId")]
        public int? SubPlantId { get; set; }

        [ForeignKey("SubModuleId")]
        public int? SubModuleId { get; set; }

        public bool IsActive { get; set; }
        public int? TenantId { get; set; }

        [ForeignKey("ChecklistTypeId")]
        public ICollection<VehicleInspectionHeader> VehicleInspectionHeader { get; set; }

        [ForeignKey("ChecklistTypeId")]
        public ICollection<MaterialInspectionRelationDetail> MaterialInspectionRelationDetails { get; set; }

        [ForeignKey("ChecklistTypeId")]
        public ICollection<WMCalibrationHeader> WMCalibrationHeaders { get; set; }
    }
}