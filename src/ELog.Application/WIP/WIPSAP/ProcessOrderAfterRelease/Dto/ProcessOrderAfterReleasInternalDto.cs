using Abp.Application.Services.Dto;
using Abp.AutoMapper;

using ELog.Core;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ELog.Application.WIP.WIPSAP.ProcessOrderAfterRelease.Dto
{
    [AutoMapFrom(typeof(ELog.Core.Entities.ProcessOrderAfterRelease))]
    public class ProcessOrderAfterReleasInternalDto : EntityDto<int>
    {
        [Required(ErrorMessage = "Plant Id is required.")]
        public int PlantId { get; set; }

        [StringLength(PMMSConsts.Medium)]
        [Required(ErrorMessage = "Process Order Number is required.")]
        public string ProcessOrderNo { get; set; }

        [StringLength(PMMSConsts.Medium)]
        [Required(ErrorMessage = "Process Order Type is required.")]
        public string ProcessOrderType { get; set; }

        [Required(ErrorMessage = "Process Order Date is required.")]
        public DateTime ProcessOrderDate { get; set; }

        [StringLength(PMMSConsts.Medium)]
        [Required(ErrorMessage = "Product Code is required.")]
        public string ProductCode { get; set; }
        [Required(ErrorMessage = "Product Id is required.")]
        public int ProductCodeId { get; set; }
        public Boolean TecoFlag { get; set; }
        public List<ProcessOrderMaterialAfterReleasInternalDto> ListOfMaterials { get; set; }
    }
}
