using ELog.Core.Authorization;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ELog.Core.Entities
{
    [Table("EquipmentUsageLog")]
    public class EquipmentUsageLog : PMMSFullAudit
    {
        [ForeignKey("ActivityMaster")]
        public int ActivityId { get; set; }

        public string OperatorName { get; set; }
        public string EquipmentType { get; set; }

        [ForeignKey("EquipmentMaster")]
        public int EquipmentBracodeId { get; set; }

        [ForeignKey("CubicleMaster")]
        public int ProcessBarcodeId { get; set; }
        public string Remarks { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public bool IsActive { get; set; }
        public bool Status { get; set; }

        public int? ApprovedBy { get; set; }
        public DateTime? ApprovedTime { get; set; }
        public int? StatusId { get; set; }

        [ForeignKey("EquipmentUsageHeaderId")]
        public ICollection<EquipmentUsageLogList> EquipmentUsageLogLists { get; set; }

    }
}
