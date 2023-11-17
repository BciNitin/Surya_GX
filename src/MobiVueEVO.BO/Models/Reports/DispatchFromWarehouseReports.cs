using System;
using System.Collections.Generic;
using System.Text;

namespace MobiVueEVO.BO.Models
{
    public class DispatchFromWarehouseReports
    {
        public string MaterialCode { get; set; }
        public DateTime? FromDate { get; set; }
        public string LineCode { get; set; }
        public string TransferOrder { get; set; }
        public string PlantCode { get; set; }
        public DateTime? ToDate { get; set; }
        public string DeliveryNo { get; set; }
    }
}
