
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ELog.Core.Entities
{
    [Table("AreaMaster")]
    public class AreaMaster : PMMSFullAuditWithApprovalStatus
    {
        [ForeignKey("PlantMaster")]
        public int SubPlantId { get; set; }

        [ForeignKey("DepartmentMaster")]
        public int DepartmentId { get; set; }

        [Required]
        [StringLength(PMMSConsts.Small)]
        public string AreaCode { get; set; }

        [Required]
        [StringLength(PMMSConsts.Medium)]
        public string AreaName { get; set; }

        public bool IsActive { get; set; }
        public string Description { get; set; }
        [StringLength(PMMSConsts.Medium)]
        public string Zone { get; set; }
        public int? TenantId { get; set; }

        [ForeignKey("AreaId")]
        public ICollection<LocationMaster> LocationMasters { get; set; }

        [ForeignKey("AreaId")]
        public ICollection<CubicleMaster> CubicleMasters { get; set; }

        [ForeignKey("AreaId")]
        public ICollection<StandardWeightBoxMaster> StandardWeightBoxMasters { get; set; }

        [ForeignKey("AreaId")]
        public ICollection<StandardWeightMaster> StandardWeightMasters { get; set; }

        [ForeignKey("AreaId")]
        public ICollection<DeviceMaster> DeviceMasters { get; set; }
    }
}