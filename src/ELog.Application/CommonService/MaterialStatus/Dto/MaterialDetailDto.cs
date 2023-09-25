using System;

namespace ELog.Application.CommonService.MaterialStatus.Dto
{
    public class MaterialDetailDto
    {
        public string MaterialCode { get; set; }
        public string MaterialDescription { get; set; }
        public string VendorCode { get; set; }
        public string ManufacturerCode { get; set; }
        public string MfgBatchNo { get; set; }
        public DateTime? QCInvDate { get; set; }
        public string SapBatchNo { get; set; }
        public DateTime? MgfDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public int DaysLeftForExpiry { get; set; }
        public string Status { get; set; }
        public DateTime? RetestDate { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public int DaysLeftForRetest { get; set; }
        public string GRNNo { get; set; }
        public string ArNo { get; set; }
        public float NoOfContainer { get; set; }
        public string TotalQtyReceived { get; set; }
        public string QtyIssueToProduction { get; set; }
        public float RejectedQty { get; set; }
        public string QtyReturnedFromProduction { get; set; }
        public string UOM { get; set; }
        public float ContainerNo { get; set; }

        public string DoneBy { get; set; }
        public DateTime? SamplingDate { get; set; }
        public string ReleasedBy { get; set; }

        public string BatchNo { get; set; }
        public string ProcessOrder { get; set; }
        public string TempStatus { get; set; }

    }
}
