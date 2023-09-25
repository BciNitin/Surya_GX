
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ELog.Core.Entities
{
    [Table("DepartmentMaster")]
    public class DepartmentMaster : PMMSFullAuditWithApprovalStatus
    {
        [ForeignKey("PlantMaster")]
        public int SubPlantId { get; set; }

        [Required]
        [StringLength(PMMSConsts.Small)]
        public string DepartmentCode { get; set; }

        [Required]
        [StringLength(PMMSConsts.Medium)]
        public string DepartmentName { get; set; }

        public bool IsActive { get; set; }
        public string Description { get; set; }
        public int? TenantId { get; set; }

        [ForeignKey("DepartmentId")]
        public ICollection<StandardWeightBoxMaster> StandardWeightBoxMasters { get; set; }

        [ForeignKey("DepartmentId")]
        public ICollection<StandardWeightMaster> StandardWeightMasters { get; set; }

        [ForeignKey("DepartmentId")]
        public ICollection<AreaMaster> AreaMasters { get; set; }

        [ForeignKey("DepartmentId")]
        public ICollection<DeviceMaster> DeviceMasters { get; set; }

        [ForeignKey("SLOCId")]
        public ICollection<CubicleMaster> CubicleMasters { get; set; }

        [ForeignKey("SLOCId")]
        public ICollection<EquipmentMaster> EquipmentMasters { get; set; }
    }
}