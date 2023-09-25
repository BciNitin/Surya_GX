using System.Collections.Generic;

namespace ELog.Core.SAP
{
    public class IssueToProductionRequestResponseDto
    {
        public List<IssueToProductionRecord> IssueToProductionRecords { get; set; }
    }

    public class IssueToProductionRecord
    {
        public string ProcessOrderNo { get; set; }
        public string MaterialCode { get; set; }
        public string LineItemNo { get; set; }
        public string MaterialDescription { get; set; }
        public string Product { get; set; }
        public string ProductBatch { get; set; }
        public string BatchNo { get; set; }
        public string ArNo { get; set; }
        public string SAPBatchNo { get; set; }
        public string DispensedQty { get; set; }
        public string UOM { get; set; }
        public string Storage_location { get; set; }
        public string MvtType { get; set; }
        public string MaterialIssueNoteNo { get; set; }
    }
}