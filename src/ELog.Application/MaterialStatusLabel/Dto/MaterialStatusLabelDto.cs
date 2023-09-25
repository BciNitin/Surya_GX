using System;

namespace ELog.Application.MaterialStatusLabel.Dto
{
    public class MaterialStatusLabelDto
    {
        public string MaterialCode { get; set; }
        public string InspectionLotNo { get; set; }
        public string MaterialDescription { get; set; }
        public string ManfactureCode { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public string ManufacturerBatchNo { get; set; }
        public string SapBatchNo { get; set; }
        public DateTime? MfgDate { get; set; }
        public DateTime? ExpDate { get; set; }
        public DateTime? MfgrRetestDate { get; set; }
        public string GRNNo { get; set; }
        public float NoOfContainer { get; set; }
        public string GrnPreparedBy { get; set; }
        public string VendorCode { get; set; }
        public string PackSize { get; set; }
        public string Status { get; set; }
        public string ArNo { get; set; }
        public string RejectedQty { get; set; }
        public string ReleasedQty { get; set; }
        public string StatusColorCode { get; set; }
        public string TotalQtyReceived { get; set; }
        public string ExpiredQty { get; set; }
        public bool IsExpired { get; set; }
        public bool IsRejected { get; set; }
        public bool IsQcReleased { get; set; }
        public DateTime? InHouseRetestDate { get; set; }
        public int? PlantId { get; set; }
    }
}