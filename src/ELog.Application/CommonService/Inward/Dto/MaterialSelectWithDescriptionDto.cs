using Abp.Application.Services.Dto;

namespace ELog.Application.CommonService.Inward.Dto
{
    public class MaterialSelectWithDescriptionDto : EntityDto<int>
    {
        public string Value { get; set; }
        public string Description { get; set; }
        public int? SelfLife { get; set; }

        public int PurchaseOrderDeliverSchedule { get; set; }
    }
}