using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using ELog.Core.Entities;

namespace ELog.Application.WIP.WeightVerification.Dto
{
    [AutoMapFrom(typeof(WeightVerificationDetail))]
    public class WeightVerificationDetailDto : EntityDto<int>
    {
        public int WeightVerificationHeaderId { get; set; }

        public int? NoOfContainers { get; set; }
        public int? NoOfPacks { get; set; }
        public int? RecivedNoOfPacks { get; set; }
        public int ScanBalanceId { get; set; }
        public float? GrossWeight { get; set; }
        public float? NetWeight { get; set; }
        public float? TareWeight { get; set; }
    }
}
