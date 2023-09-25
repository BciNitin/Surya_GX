using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ELog.Core.Entities
{
    [Table("CalenderMaster")]
    public class CalenderMaster : PMMSFullAuditWithApprovalStatus
    {
        public int SubPlantId { get; set; }
        public DateTime CalenderDate { get; set; }

        [ForeignKey("HolidayTypeId")]
        public int HolidayTypeId { get; set; }

        [StringLength(PMMSConsts.Medium)]
        public string HolidayName { get; set; }

        public string Description { get; set; }
        public bool IsActive { get; set; }
        public int? TenantId { get; set; }
    }
}