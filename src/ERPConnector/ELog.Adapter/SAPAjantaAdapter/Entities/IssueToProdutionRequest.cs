using System.Collections.Generic;

namespace ELog.Adapter.SAPAjantaAdapter.Entities
{
    public class IssueToProdutionRequest
    {
        public List<IssueToProductionRequestRecord> Record { get; set; }
    }
    public class IssueToProductionRequestRecord
    {
        public string ProcessOrderNo { get; set; }
        public string MaterialCode { get; set; }
        public string MaterialDescription { get; set; }
        public string Product { get; set; }
        public string ProductBatchNo { get; set; }
        public string DispensedQty { get; set; }
        public string UOM { get; set; }
        public string StorageLocation { get; set; }
        public string MvtType { get; set; }
        public string SapBatchNo { get; set; }
    }
}