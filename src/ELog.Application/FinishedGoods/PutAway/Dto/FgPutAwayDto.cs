using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using ELog.Core.Entities;

namespace ELog.Application.FinishedGoods.PutAway.Dto
{

    [AutoMapFrom(typeof(FgPutAway))]
    public class FgPutAwayDto : EntityDto<int>
    {
        public int? PalletId { get; set; }
        public string PalletBarcode { get; set; }
        public int PalletCount { get; set; }
        public int? LocationId { get; set; }
        public string LocationBarcode { get; set; }
        public bool isActive { get; set; }
        public bool isPicked { get; set; }
        public string HUCode { get; set; }
        public int? PlantId { get; set; }
        public string ProductBatchNo { get; set; }

    }
}
