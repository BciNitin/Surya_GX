using ELog.Core.Authorization;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ELog.Core.Entities
{
    [Table("DispensingHeaders")]
    public class DispensingHeader : PMMSFullAudit
    {
        [Required]
        [ForeignKey("RLAFId")]
        public int RLAFId { get; set; }

        [ForeignKey("ProcessOrderId")]
        public int? ProcessOrderId { get; set; }

        [ForeignKey("InspectionLotId")]
        public int? InspectionLotId { get; set; }

        [Required]
        [StringLength(PMMSConsts.Medium)]
        public string MaterialCodeId { get; set; }

        [Required]
        public DateTime StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        [Required]
        [ForeignKey("StatusId")]
        public int StatusId { get; set; }
        public bool IsSampling { get; set; }

        public string CheckedBy { get; set; }

        public int? DoneBy { get; set; }
        public int? CheckedById { get; set; }
        public int? TenantId { get; set; }



        [ForeignKey("DispensingHeaderId")]
        public ICollection<DispensingDetail> DispensingDetails { get; set; }
    }
}