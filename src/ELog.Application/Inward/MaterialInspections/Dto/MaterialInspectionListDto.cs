using Abp.Application.Services.Dto;

namespace ELog.Application.Inward.MaterialInspections.Dto
{
    public class MaterialInspectionListDto : EntityDto<int>
    {
        public int? GateEntryId { get; set; }

        public int? PurchaseOrderId { get; set; }

        public string GatePassNo { get; set; }

        public string PurchaseOrderNo { get; set; }

        public string InvoiceNo { get; set; }

        public string LRNo { get; set; }
        public int SubPlantId { get; set; }
        public int? TransactionStatusId { get; set; }
        public string UserEnteredTransaction { get; set; }
        public string PurchaseOrderDeliverSchedule { get; set; }
    }
}