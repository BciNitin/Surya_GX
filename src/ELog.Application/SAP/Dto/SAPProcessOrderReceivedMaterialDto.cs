using Abp.AutoMapper;

using ELog.Core.Entities;

namespace ELog.Application.SAP.Dto
{
    [AutoMap(typeof(SAPProcessOrderReceivedMaterial))]
    public class SAPProcessOrderReceivedMaterialDto
    {
        public string Plant { get; set; }
        public string PONo { get; set; }
        public string PODate { get; set; }
        public string LineItemNo { get; set; }
        public decimal? OrderQty { get; set; }
        public string UOM { get; set; }
        public string ItemCode { get; set; }
        public string ItemDescription { get; set; }
        public string VendorName { get; set; }
        public string VendorCode { get; set; }
        public string ManufacturerName { get; set; }
        public string ManufacturerCode { get; set; }
    }
}