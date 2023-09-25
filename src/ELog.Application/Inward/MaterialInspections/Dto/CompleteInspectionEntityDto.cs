using Abp.Application.Services.Dto;

namespace ELog.Application.Inward.MaterialInspections.Dto
{
    public class CompleteInspectionEntityDto : EntityDto<int>
    {
        public int PurchaseOrderId { get; set; }
    }
}