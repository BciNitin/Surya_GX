using Abp.Application.Services.Dto;

using System;
using System.Collections.Generic;

namespace ELog.Application.Inward.GRNPostings.Dto
{
    public class GRNMaterialLabelPrintingDto : EntityDto<int>
    {
        public int GRNHeaderId { get; set; }
        public int MaterialLabelPrintHeaderId { get; set; }
        public bool IsAlreadyPrinted { get; set; }
        public string MaterialCode { get; set; }
        public string MaterialDescription { get; set; }
        public string ManufacturingBatchNo { get; set; }
        public string SAPBatchNo { get; set; }
        public DateTime? ManufacturingDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public DateTime? RetestDate { get; set; }
        public string PackDetails { get; set; }
        public float NumberOfContainers { get; set; }
        public float TotalQty { get; set; }
        public int? PrinterId { get; set; }
        public string Comment { get; set; }
        public string RangePrint { get; set; }
        public IEnumerable<MaterialLabelGRNQuantityDto> lstMaterialLabelGRNQuantity { get; set; }
    }

    public class GRNLabelPrintingVendorDetail
    {
        public string VendorName { get; set; }

        public string VendorCode { get; set; }

        public string ManufacturerCode { get; set; }
        public string ManufacturerName { get; set; }
        public string PlantId { get; set; }
        public string PlantName { get; set; }
        public string InvoiceNo { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public string GrnNumber { get; set; }
        public DateTime? GrnDate { get; set; }
        public string InspectionLotNo { get; set; }

        public string MfgBatchNo { get; set; }

        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string UOM { get; set; }

        public DateTime? ManufacturingDate { get; set; }

        public DateTime? ExpiryDate { get; set; }
        public DateTime? RetestDate { get; set; }






    }

    public class MaterialLabelGRNQuantityDto
    {
        public int GRNQuantityId { get; set; }
        public float NumberOfContainers { get; set; }
        public float TotalQty { get; set; }
        public float QtyPerContainer { get; set; }
    }
}