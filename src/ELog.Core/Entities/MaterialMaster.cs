using ELog.Core.Authorization;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ELog.Core.Entities
{
    [Table("MaterialMaster")]
    public class MaterialMaster : PMMSFullAudit
    {
        [StringLength(PMMSConsts.Small)]
        [Required]
        public string MaterialCode { get; set; }

        public string MaterialDescription { get; set; }

        [StringLength(PMMSConsts.Small)]
        [Required]
        public string BaseUOM { get; set; }

        [StringLength(PMMSConsts.Small)]
        [Required]
        public string Grade { get; set; }

        [Required]
        public float Denominator { get; set; }

        [Required]
        public float Numerator { get; set; }

        [StringLength(PMMSConsts.Small)]
        [Required]
        public string ConversionUOM { get; set; }

        [StringLength(PMMSConsts.Small)]
        [Required]
        public string MaterialType { get; set; }

        public int? Flag { get; set; }
        public string TempStatus { get; set; }

    }
}