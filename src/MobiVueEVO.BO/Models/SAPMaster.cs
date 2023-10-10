using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MobiVueEVO.BO.Models
{
    public class SAPMaster
    {
        public class PLANTMASTER
        {
            [Required]
            public string PlantCode { get; set; }
            [Required]
            public string Description { get; set; }
            [Required]
            public string Type { get; set; }
            [Required]
            public string Address { get; set; }          
        }
        public class CUSTOMERMASTER
        {
            [Required]
            public string CustomerCode { get; set; }
            [Required]
            public string CustomerName { get; set; }
            [Required]
            public string Address { get; set; }
            [Required]
            public string City { get; set; }
            [Required]
            public string District { get; set; }
        }
        public class WORKLINEMASTER
        {
            [Required]
            public string PlantCode { get; set; }
            [Required]
            public string WorkCenterCode { get; set; }
            [Required]
            public string Work_Center_Discription { get; set; }
            [Required]
            public string Status { get; set; }
            
        }
        public class STORAGELOCATION
        {
            [Required]
            public string Code { get; set; }
            [Required]
            public string StrLocCode { get; set; }
            [Required]
            public string Description { get; set; }
            
        }
        public class PACKINGORDER
        {
            [Required]
            public string PlantCode { get; set; }
            [Required]
            public string PackingOrderNumber { get; set; }
            [Required]
            public string MaterialCode { get; set; }
            [Required]
            public int Quantity { get; set; }
            [Required]
            public string StorageLocation { get; set; }
            [Required]
            public string Packing_Date { get; set; }
            [Required]
            public string WorkCenter { get; set; }
            [Required]
            public string BOM { get; set; }
        }

    }
}
