using System;
using System.Collections.Generic;
using System.Text;

namespace MobiVueEVO.BO.Models
{
    public class QualityConfirmation
    {
        public string PlantCode { get; set; }
        public string MaterialCode { get; set; }
        public string StrLocCode { get; set; }
        public string BatchCode { get; set; }
        public string PackingOrderNo { get; set; }
        public decimal PackedQty { get; set; }
        public DateTime QCDate { get; set; }
        public string LineNo { get; set; }
        public decimal OKQty { get; set; } 
        public decimal NGQty { get; set; }

    }
}
