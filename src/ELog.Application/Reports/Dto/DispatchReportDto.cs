using System;
using System.Collections.Generic;
using System.Text;

namespace ELog.Application.Reports.Dto
{
    public class DispatchReportDto
    {
        public DateTime? CreatedOn { get; set; }
        public long? CreatedBy { get; set; }
        public string OBD { get; set; }
        public int? ProductId { get; set; }
        public int? PutawayId { get; set; }
        public int? PickingId { get; set; }
        public string ProductBatchNo { get; set; }
        public string ProductName { get; set; }
        public string ProductCode { get; set; }
        public string LineItem { get; set; }
        public string Batch { get; set; }
        public string Description { get; set; }
        public string PalletBarcode { get; set; }
        public int? PalletCount { get; set; }
        public int? Quantity { get; set; }
        public string UOM { get; set; }
        public int? NoOfPacks { get; set; }
        public string CustomerName { get; set; }
        public string CustomerAddress { get; set; }
        public string TransportName { get; set; }
        public string VehicleNo { get; set; }
        public bool? isActive { get; set; }
        public bool? isPicked { get; set; }
        public string HUCode { get; set; }
        public int? PlantId { get; set; }


    }
}