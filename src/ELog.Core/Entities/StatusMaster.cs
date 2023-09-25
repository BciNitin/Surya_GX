using ELog.Core.Authorization;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ELog.Core.Entities
{
    [Table("StatusMaster")]
    public class StatusMaster : PMMSFullAudit
    {
        [StringLength(PMMSConsts.Medium)]
        public string Status { get; set; }

        [ForeignKey("ModuleId")]
        public int ModuleId { get; set; }

        [ForeignKey("SubModuleId")]
        public int SubModuleId { get; set; }

        [ForeignKey("GroupStatusId")]
        public ICollection<CubicleAssignmentHeader> CubicleAssignmentHeaders { get; set; }

        [ForeignKey("StatusId")]
        public ICollection<CubicleAssignmentDetail> CubicleAssignmentDetails { get; set; }

        [ForeignKey("StatusId")]
        public ICollection<CubicleCleaningTransaction> CubicleCleaningTransactions { get; set; }

        [ForeignKey("StatusId")]
        public ICollection<CubicleCleaningDailyStatus> CubicleCleaningDailyStatuses { get; set; }

        [ForeignKey("StatusId")]
        public ICollection<EquipmentCleaningTransaction> EquipmentCleaningTransactions { get; set; }

        [ForeignKey("StatusId")]
        public ICollection<EquipmentCleaningStatus> EquipmentCleaningStatuses { get; set; }

        [ForeignKey("StatusId")]
        public ICollection<LineClearanceTransaction> LineClearanceTransactions { get; set; }

        [ForeignKey("StatusId")]
        public ICollection<DispensingHeader> DispensingHeaders { get; set; }

        [ForeignKey("StatusId")]
        public ICollection<StageOutHeader> StageOutHeader { get; set; }
        [ForeignKey("StatusId")]
        public ICollection<ReturnToVendorHeader> ReturnToVendorHeaders { get; set; }
    }
}