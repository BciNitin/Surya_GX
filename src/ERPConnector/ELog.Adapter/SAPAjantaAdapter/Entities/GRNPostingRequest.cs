using System.Collections.Generic;

namespace ELog.Adapter.SAPAjantaAdapter.Entities
{
    public class GRNRequest
    {
        public string ItemCode { get; set; }
        public string VendorCode { get; set; }
        public string VendorBatch { get; set; }
        public string NetQty { get; set; }
        public string UOM { get; set; }
        public string PurchaseOrder { get; set; }
        public string POLineItem { get; set; }
        public string LRNo { get; set; }
        public string LRDate { get; set; }
        public string Vehicle { get; set; }
        public string NoOfCases { get; set; }
        public string TransporterName { get; set; }
        public string MfgDate { get; set; }
        public string ExpDate { get; set; }
        public string Manufacturer { get; set; }
        public string Delivery_note_no { get; set; }
        public string Bill_of_lading { get; set; }
        public string Storage_location { get; set; }
        public string LineItem { get; set; }
        public string InvoiceNo { get; set; }
        public string InvoiceQty { get; set; }
        public string No_Of_Containers { get; set; }

        public string GRNPreparedBy { get; set; }
        public string Remark { get; set; }
        public string QtyPerContainer { get; set; }
    }

    public class GRNPostingRequest
    {
        public List<GRNRequest> Record { get; set; }
    }
}