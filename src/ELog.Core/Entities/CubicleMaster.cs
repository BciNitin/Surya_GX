using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ELog.Core.Entities
{
    [Table("CubicleMaster")]
    public class CubicleMaster : PMMSFullAuditWithApprovalStatus
    {
        [ForeignKey("PlantId")]
        public int PlantId { get; set; }

        [StringLength(PMMSConsts.Small)]
        public string CubicleCode { get; set; }

        [ForeignKey("AreaMaster")]
        public int AreaId { get; set; }

        [ForeignKey("SLOCId")]
        public int? SLOCId { get; set; }

        public bool IsActive { get; set; }
        public string Description { get; set; }
        public int? TenantId { get; set; }

        [ForeignKey("CubicleId")]
        public ICollection<DeviceMaster> DeviceMasters { get; set; }

        [ForeignKey("CubicleId")]
        public ICollection<CubicleCleaningTransaction> CubicleCleaningTransactions { get; set; }

        [ForeignKey("CubicleId")]
        public ICollection<CubicleCleaningDailyStatus> CubicleCleaningDailyStatuses { get; set; }

        [ForeignKey("CubicleId")]
        public ICollection<CubicleAssignmentDetail> CubicleAssignmentDetails { get; set; }

        [ForeignKey("Cubicleid")]
        public ICollection<EquipmentAssignment> EquipmentAssignments { get; set; }

        [ForeignKey("CubicleId")]
        public ICollection<LineClearanceTransaction> LineClearanceTransactions { get; set; }

        [ForeignKey("CubicleId")]
        public ICollection<StageOutHeader> StageOutHeaders { get; set; }
    }
}