using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MobiVueEVO.BO.Models
{
    public class QualityCheckingModel
    {
        
        public string PlantCode { get; set; }

       
        public string LineCode { get; set; }

       
        public string PackingOrderNo { get; set; }

       
        public string Status { get; set; }

       
        public string ParentBarcode { get; set; }

       
        public string ChildBarcode { get; set; }

        public string QCStatus { get; set; }


    }

    //public string parentBarcode { get; set; }
    //public string itemBarCode { get; set; }
    //public string status { get; set; }

}
