using ELog.Core;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ELog.Application.SAP.MaterialMaster.Dto
{
    public class SAPMaterial
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
        [Range(1, float.MaxValue, ErrorMessage = "Please enter a value bigger than {1}")]
        public float Denominator { get; set; }

        [Required]
        [Range(1, float.MaxValue, ErrorMessage = "Please enter a value bigger than {1}")]
        public float Numerator { get; set; }

        [StringLength(PMMSConsts.Small)]
        [Required]
        public string ConversionUOM { get; set; }

        [StringLength(PMMSConsts.Small)]
        [Required]
        public string MaterialType { get; set; }

        public int? Flag { get; set; }
    }

    public class SAPMaterials
    {
        public List<SAPMaterial> Record { get; set; }
    }
}