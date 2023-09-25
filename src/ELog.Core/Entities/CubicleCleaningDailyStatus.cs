using ELog.Core.Authorization;

using System;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace ELog.Core.Entities
{
    [Table("CubicleCleaningDailyStatus")]
    public class CubicleCleaningDailyStatus : PMMSFullAudit
    {
        [Required]
        public DateTime CleaningDate { get; set; }
        [ForeignKey("CubicleId")]
        public int CubicleId { get; set; }
        [ForeignKey("StatusId")]
        public int StatusId { get; set; }
        public bool IsSampling { get; set; }
        public int? TenantId { get; set; }
    }
}
