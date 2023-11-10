using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json.Serialization;

namespace MobiVueEVO.BO.Models
{
    public class PackingOrderBarcodeDetails
    {
        public string MaterialCode { get; set; }
        public string ShiperBarcode { get; set; }
        public DateTime? FromDate { get; set; }
        public string LineCode { get; set; }
        public string PackingOrder { get; set; }
        public string PlantCode { get; set; }
        public DateTime? ToDate { get; set; }

    }
}
