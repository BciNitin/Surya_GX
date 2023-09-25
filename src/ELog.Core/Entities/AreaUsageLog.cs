using ELog.Core.Authorization;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ELog.Core.Entities
{
    [Table("AreaUsageLog")]
    public class AreaUsageLog : PMMSFullAudit
    {
        [ForeignKey("ActivityMaster")]
        public int ActivityID { get; set; }

        [ForeignKey("CubicleMaster")]
        public int CubicalId { get; set; }
        public string OperatorName { get; set; }

        public DateTime? StartTime { get; set; }

        public DateTime? StopTime { get; set; }

        public string Remarks { get; set; }

        public int? ApprovedBy { get; set; }
        public int? VerifiedBy { get; set; }
        public DateTime? ApprovedTime { get; set; }
        public int? StatusId { get; set; }

        public bool IsActive { get; set; }
        public bool Status { get; set; }
        [ForeignKey("AreaUsageHeaderId")]
        public ICollection<AreaUsageListLog> AreaUsageLogLists { get; set; }
    }
}
