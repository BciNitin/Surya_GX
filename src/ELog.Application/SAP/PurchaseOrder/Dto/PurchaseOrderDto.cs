using ELog.Core;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ELog.Application.SAP.PurchaseOrder.Dto
{
    public class PurchaseOrderDto
    {
        [Required(ErrorMessage = "Plant Id is required.")]
        public string PlantId { get; set; }

        [StringLength(PMMSConsts.Medium)]
        [Required(ErrorMessage = "Purchase Order Number is required.")]
        public string PurchaseOrderNo { get; set; }

        [Required(ErrorMessage = "Purchase Order Date is required.")]
        public DateTime PurchaseOrderDate { get; set; }

        [StringLength(PMMSConsts.Medium)]
        [Required(ErrorMessage = "Vendor Name is required.")]
        public string VendorName { get; set; }
        [StringLength(PMMSConsts.Medium)]
        [Required(ErrorMessage = "Vendor code is required.")]
        public string VendorCode { get; set; }
        public string ManufacturerName { get; set; }
        public string ManufacturerCode { get; set; }

        public List<MaterialDto> ListOfMaterials { get; set; }
    }
}