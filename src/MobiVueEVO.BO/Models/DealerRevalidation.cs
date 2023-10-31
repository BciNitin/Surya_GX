using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MobiVueEVO.BO.Models
{
    public class DealerRevalidation
    {

        public string DealerCode { get; set; }
        public string ItemBarCode { get; set; }
        public string ParentBarCode { get; set; }
        public string MaterialCode { get; set; }
        public string BatchCode { get; set; }
        public DateTime PackingDate { get; set; }
        public decimal Qty { get; set; }
    }
}
