using System;
using System.Collections.Generic;
using System.Text;

namespace MobiVueEVO.BO.Models
{
    public class GenerateSerialNumber
    {
        public int Id { get; set;}
        public string PlantCode { get; set;}
        public string LineCode { get; set;}
        public string PackingOrderNo { get; set;}
        public string SupplierCode { get; set;}
        public string DriverCode { get; set;}
        public string UserId { get; set; }
        public double Quantity { get; set; }
        public double PrintedQty { get; set; }
        public double PendingQtyToPrint { get; set; }
        public DateTime PackingDate { get; set; }
        public string ItemCode { get; set; }    

    }
}
