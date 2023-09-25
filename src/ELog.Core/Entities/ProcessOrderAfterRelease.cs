using ELog.Core.Authorization;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ELog.Core.Entities
{
    [Table("ProcessOrderAfterRelease")]
    public class ProcessOrderAfterRelease : PMMSFullAudit

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
        [Required]
        public int ProductCodeId { get; set; }
        public Boolean TecoFlag { get; set; }

        public Boolean IsActive { get; set; }

        public Boolean IsPicking { get; set; }


    }
}
