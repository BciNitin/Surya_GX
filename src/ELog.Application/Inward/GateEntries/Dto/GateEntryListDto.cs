using Abp.Application.Services.Dto;
using Abp.AutoMapper;

using ELog.Core.Entities;

namespace ELog.Application.Inward.GateEntries.Dto
{
    [AutoMapFrom(typeof(GateEntry))]
    public class GateEntryListDto : EntityDto<int>
    {
        public string GatePassNumber { get; set; }
        public int PurchaseOrderId { get; set; }
        public string UserEnteredPurchaseOrderNumber { get; set; }

        public bool IsActive { get; set; }
        public int SubPlantId { get; set; }
        public string PurchaseOrderDeliverSchedule { get; set; }
    }
}