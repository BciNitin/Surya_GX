using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MobiVueEVO.BO.Models
{
    public class QualityChecking
    {
        
        public string PlantCode { get; set; }

       
        public string LineCode { get; set; }

       
        public string PackingOrderNo { get; set; }

       
        public string QCStatus { get; set; }

       
        public string CartonBarCode { get; set; }

       
        public string ItemBarCode { get; set; }
    }

    public class QualityCheckingList
    {
      public QualityChecking[] QualityChecking { get; set; }
    }

}
