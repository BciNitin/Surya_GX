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
            [RegularExpression(@"^[^!@#$%^&*]+$", ErrorMessage = "Invalid Characters Detected.")]
            public string PlantCode { get; set; }
            [Required]
            [RegularExpression(@"^[^!@#$%^&*]+$", ErrorMessage = "Invalid Characters Detected.")]
            public string Description { get; set; }
            [Required]
            [RegularExpression(@"^[^!@#$%^&*]+$", ErrorMessage = "Invalid Characters Detected.")]
            public string Type { get; set; }
            [Required]
            [RegularExpression(@"^[^!@#$%^&*]+$", ErrorMessage = "Invalid Characters Detected.")]
            public string Address { get; set; }          
        }
        public class CUSTOMERMASTER
        {
            [Required]
            [RegularExpression(@"^[^!@#$%^&*]+$", ErrorMessage = "Invalid Characters Detected.")]
            public string CustomerCode { get; set; }
            [Required]
            [RegularExpression(@"^[^!@#$%^&*]+$", ErrorMessage = "Invalid Characters Detected.")]
            public string CustomerName { get; set; }
            [Required]
            [RegularExpression(@"^[^!@#$%^&*]+$", ErrorMessage = "Invalid Characters Detected.")]
            public string Address { get; set; }
            [Required]
            [RegularExpression(@"^[^!@#$%^&*]+$", ErrorMessage = "Invalid Characters Detected.")]
            public string City { get; set; }
            [Required]
            [RegularExpression(@"^[^!@#$%^&*]+$", ErrorMessage = "Invalid Characters Detected.")]
            public string District { get; set; }
        }
        public class WORKLINEMASTER
        {
            [Required]
            [RegularExpression(@"^[^!@#$%^&*]+$", ErrorMessage = "Invalid Characters Detected.")]
            public string PlantCode { get; set; }
            [Required]
            [RegularExpression(@"^[^!@#$%^&*]+$", ErrorMessage = "Invalid Characters Detected.")]
            public string WorkCenterCode { get; set; }
            [Required]
            [RegularExpression(@"^[^!@#$%^&*]+$", ErrorMessage = "Invalid Characters Detected.")]
            public string Work_Center_Discription { get; set; }
            [Required]
            [RegularExpression(@"^[^!@#$%^&*]+$", ErrorMessage = "Invalid Characters Detected.")]
            public string Status { get; set; }
            
        }
        public class STORAGELOCATION
        {
            [Required]
            [RegularExpression(@"^[^!@#$%^&*]+$", ErrorMessage = "Invalid Characters Detected.")]
            public string Code { get; set; }
            [Required]
            [RegularExpression(@"^[^!@#$%^&*]+$", ErrorMessage = "Invalid Characters Detected.")]
            public string StrLocCode { get; set; }
            [Required]
            [RegularExpression(@"^[^!@#$%^&*]+$", ErrorMessage = "Invalid Characters Detected.")]
            public string Description { get; set; }
            
        }
        public class PACKINGORDER
        {
            [Required]
            [RegularExpression(@"^[^!@#$%^&*]+$", ErrorMessage = "Invalid Characters Detected.")]
            public string PlantCode { get; set; }
            [Required]
            [RegularExpression(@"^[^!@#$%^&*]+$", ErrorMessage = "Invalid Characters Detected.")]
            public string PackingOrderNumber { get; set; }
            [Required]
            [RegularExpression(@"^[^!@#$%^&*]+$", ErrorMessage = "Invalid Characters Detected.")]
            public string MaterialCode { get; set; }
            [Required]
            public int Quantity { get; set; }
            [Required]
            [RegularExpression(@"^[^!@#$%^&*]+$", ErrorMessage = "Invalid Characters Detected.")]
            public string StorageLocation { get; set; }
            [Required]
            
            public string Packing_Date { get; set; }
            [Required]
            //[RegularExpression(@"^\d{4}-\d{2}-\d{2}$", ErrorMessage = "Invalid Date Format. Use YYYY-MM-DD.")]
            public string WorkCenter { get; set; }
            [Required]
            [RegularExpression(@"^[^!@#$%^&*]+$", ErrorMessage = "Invalid Characters Detected.")]
            public string BOM { get; set; }
        }
        public class SODelivery
        {
            [Required]
            [RegularExpression(@"^[^!@#$%^&*]+$", ErrorMessage = "Invalid Characters Detected.")]
            public string DeliveryChallanno { get; set; }
            [Required]
            [RegularExpression(@"^[^!@#$%^&*]+$", ErrorMessage = "Invalid Characters Detected.")]
            public string FromPlantCode { get; set; }
            [Required]
            [RegularExpression(@"^[^!@#$%^&*]+$", ErrorMessage = "Invalid Characters Detected.")]
            public string SONo { get; set; }
            [Required]
            [RegularExpression(@"^[^!@#$%^&*]+$", ErrorMessage = "Invalid Characters Detected.")]
            public string SODate { get; set; }
            [Required]
            [RegularExpression(@"^[^!@#$%^&*]+$", ErrorMessage = "Invalid Characters Detected.")]
            public int Customercode { get; set; }
            [Required]
            [RegularExpression(@"^[^!@#$%^&*]+$", ErrorMessage = "Invalid Characters Detected.")]
            public string MaterialCode { get; set; }
            [Required]
            [RegularExpression(@"^[^!@#$%^&*]+$", ErrorMessage = "Invalid Characters Detected.")]
            public string Fromstorage { get; set; }
            [Required]
            [RegularExpression(@"^[^!@#$%^&*]+$", ErrorMessage = "Invalid Characters Detected.")]
            public string BatchCode { get; set; }
            [Required]
            [RegularExpression(@"^[0-9]+$", ErrorMessage = "Only Numbers Are Allowed.")]
            public string QTY { get; set; }
            [Required]
            [RegularExpression(@"^[0-9]+$", ErrorMessage = "Only Numbers Are Allowed.")]
            public string NoofBoxes { get; set; }
        }
        public class STODelivery
        {
            public string DeliveryChallanNo { get; set; }
            public string SalesOrderNo { get; set; }
            public DateTime SODate { get; set; }
            public string SendingPlantCode { get; set; }
            public string ReceivingPlantCode { get; set; }
            public string MaterialCode { get; set; }
            public string StorageLoc { get; set; }
            public int Quantity { get; set; }
        }
        public class MATERIALMASTER
        {
            [Required]
            [RegularExpression(@"^[^!@#$%^&*]+$", ErrorMessage = "Invalid Characters Detected.")]
            public string MaterialCode { get; set; }
            [Required]
            [RegularExpression(@"^[^!@#$%^&*]+$", ErrorMessage = "Invalid Characters Detected.")]
            public string MaterialDescription { get; set; }
            [Required]
            [RegularExpression(@"^[0-9]+$", ErrorMessage = "Only Numbers Are Allowed.")]
            public string PackSize { get; set; }
            [Required]
            [RegularExpression(@"^[^!@#$%^&*]+$", ErrorMessage = "Invalid Characters Detected.")]
            public string UOM { get; set; }
            [Required]
            //[RegularExpression(@"^[0-9]+(\.[0-9]+)?$", ErrorMessage = "Invalid Input. Only Numbers And Decimals Are Allowed.")]
            public decimal UnitWeight { get; set; }
            [Required]
            public string SelfLife { get; set; }
            [Required]
            [RegularExpression("^[01]+$", ErrorMessage = "Invalid Characters Detected. Only 0 And 1 Are Allowed.")]
            public string Active { get; set; }
        }

    }
}
