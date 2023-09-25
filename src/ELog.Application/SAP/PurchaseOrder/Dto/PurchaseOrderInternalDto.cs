using Abp.Application.Services.Dto;
using Abp.AutoMapper;

using ELog.Core;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ELog.Application.SAP.PurchaseOrder.Dto
{
    [AutoMapFrom(typeof(ELog.Core.Entities.PurchaseOrder))]
    public class PurchaseOrderInternalDto : EntityDto<int>
    {
        [Required]
        public int PlantId { get; set; }

        [StringLength(PMMSConsts.Medium)]
        [Required]
        public string PurchaseOrderNo { get; set; }

        [Required]
        public DateTime PurchaseOrderDate { get; set; }

        [StringLength(PMMSConsts.Medium)]
        [Required]
        public string VendorName { get; set; }
        [StringLength(PMMSConsts.Medium)]
        [Required]
        public string VendorCode { get; set; }
        public string ManufacturerName { get; set; }//Need to remove
        public string ManufacturerCode { get; set; }//Need to remove
        public List<MaterialInternalDto> Materials { get; set; }
        public int PurchaseOrderDeliverSchedule { get; set; }
    }
}