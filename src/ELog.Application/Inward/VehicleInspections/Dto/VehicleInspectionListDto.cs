using Abp.Application.Services.Dto;
using Abp.AutoMapper;

using ELog.Core.Entities;

namespace ELog.Application.Inward.VehicleInspections.Dto
{
    [AutoMapFrom(typeof(VehicleInspectionHeader))]
    public class VehicleInspectionListDto : EntityDto<int>
    {
        public int? GateEntryId { get; set; }

        public int? PurchaseOrderId { get; set; }

        public string GatePassNo { get; set; }

        public string PurchaseOrderNo { get; set; }

        public string InvoiceNo { get; set; }

        public string LRNo { get; set; }
        public int SubPlantId { get; set; }
        public int? TransactionStatusId { get; set; }
        public string TransactionStatus { get; set; }
        public string PurchaseOrderDeliverSchedule { get; set; }
    }
}