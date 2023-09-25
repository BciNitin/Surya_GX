using ELog.Core.Authorization;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ELog.Core.Entities
{
    [Table("ProcessOrders")]
    public class ProcessOrder : PMMSFullAudit
    {
        [ForeignKey("PlantId")]
        [Required]
        public int PlantId { get; set; }

        [StringLength(PMMSConsts.Medium)]
        [Required]
        public string ProcessOrderNo { get; set; }

        [StringLength(PMMSConsts.Medium)]
        public string ProcessOrderType { get; set; }

        [Required]
        public DateTime ProcessOrderDate { get; set; }

        [StringLength(PMMSConsts.Medium)]
        [Required]
        public string ProductCode { get; set; }

        [StringLength(PMMSConsts.Small)]
        public string IssueQuantityUOM { get; set; }

        [StringLength(PMMSConsts.Small)]
        public string IssueIndicator { get; set; }

        [ForeignKey("ProcessOrderId")]
        public ICollection<ProcessOrderMaterial> ProcessOrders { get; set; }

        [ForeignKey("ProcessOrderId")]
        public ICollection<CubicleAssignmentDetail> CubicleAssignmentDetails { get; set; }

        [ForeignKey("ProcessOrderId")]
        public ICollection<DispensingHeader> DispensingHeaders { get; set; }

        [Required]
        public bool IsReservationNo { get; set; }
    }
}