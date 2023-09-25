using ELog.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ELog.Application.WIP.WIPSAP.ProcessOrderAfterRelease.Dto
{
    public class ProcessOrderAfterReleasDto
    {


        [Required(ErrorMessage = "Plant Id is required.")]
        public string PlantId { get; set; }


        [StringLength(PMMSConsts.Medium)]
        [Required(ErrorMessage = "Process Order Number is required.")]
        public string ProcessOrderNo { get; set; }

        public string ProcessOrderType { get; set; }

        public DateTime ProcessOrderDate { get; set; }

        [Required(ErrorMessage = "Product Code is required.")]
        public string ProductCode { get; set; }

        public int? ProductCodeId { get; set; }

        public Boolean TecoFlag { get; set; }

        public Boolean IsPicking { get; set; }

        public List<ProcessOrderMaterialAfterReleasDto> ListOfMaterials { get; set; }

    }
}
