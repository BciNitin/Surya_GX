using System;
using System.Collections.Generic;
using System.Text;

namespace MobiVueEVO.BO.Models
{
    public class PackingOrderConfirm
    {
        public string LineCode { get; set; }
        public double PackedQty { get; set; }
        public string PackingOrderNo { get; set; }
        public DateTime Packing_Date { get; set; }
        public string PlantCode { get; set; }
        public string StrLocCode { get; set; }
    }
}
