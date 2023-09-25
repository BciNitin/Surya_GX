using System.Collections.Generic;

namespace ELog.Adapter.SAPAjantaAdapter.Entities
{
    public class GRNResponse
    {
        public string GRNNo { get; set; }
        public string ItemCode { get; set; }
        public string LineItem { get; set; }
        public string InspectionLotNo { get; set; }
        public string SAPBatchNo { get; set; }
        public string NextInspectionDate { get; set; }
    }

    public class GRNPostingResponse
    {
        public List<GRNResponse> Record { get; set; }
    }
}