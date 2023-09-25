﻿using Abp.Application.Services.Dto;
using Abp.AutoMapper;

using ELog.Core;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ELog.Application.SAP.ProcessOrder.Dto
{
    [AutoMapFrom(typeof(ELog.Core.Entities.ProcessOrder))]
    public class ProcessOrderInternalDto : EntityDto<int>
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

        [StringLength(PMMSConsts.Small)]
        public string IssueQuantityUOM { get; set; }

        [StringLength(PMMSConsts.Small)]
        public string IssueIndicator { get; set; }

        public List<ProcessOrderMaterialInternalDto> ListOfMaterials { get; set; }
    }
}