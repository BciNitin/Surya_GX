using ELog.Core.Authorization;

using System;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace ELog.Core.Entities
{
    [Table("EquipmentCleaningStatus")]
    public class EquipmentCleaningStatus : PMMSFullAudit
    {
        [Required]
        public DateTime CleaningDate { get; set; }

        [ForeignKey("EquipmentId")]
        public int EquipmentId { get; set; }

        [ForeignKey("StatusId")]
        public int StatusId { get; set; }
        public int? TenantId { get; set; }
        public bool IsSampling { get; set; }
    }
}
