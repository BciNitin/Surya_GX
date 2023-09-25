using Abp.Application.Services.Dto;
using Abp.AutoMapper;

using ELog.Core.Entities;

namespace ELog.Application.Inward.VehicleInspections.Dto
{
    [AutoMapFrom(typeof(GRNHeader))]
    public class GRNPostingListDto : EntityDto<int>
    {
        public int? PurchaseOrderId { get; set; }
        public string PurchaseOrderNo { get; set; }
        public string GRNPostingNumber { get; set; }
        public int SubPlantId { get; set; }
    }
}