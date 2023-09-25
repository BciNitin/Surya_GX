using Abp.Application.Services.Dto;
using Abp.AutoMapper;

using ELog.Core;
using ELog.Core.Entities;

using System.ComponentModel.DataAnnotations;

namespace ELog.Application.Inward.WeightCaptures.Dto
{
    [AutoMapFrom(typeof(WeightCaptureDetail))]
    public class WeightCaptureDetailsDto : EntityDto<int>
    {
        public int? WeightCaptureHeaderId { get; set; }
        public int? TenantId { get; set; }
        public int? ScanBalanceId { get; set; }
        public string GrossWeightString { get; set; }
        public float? GrossWeight { get; set; }
        public string NetWeightString { get; set; }
        public float? NetWeight { get; set; }
        public float? TareWeight { get; set; }
        public string TareWeightString { get; set; }
        public int? NoOfPacks { get; set; }
        [StringLength(PMMSConsts.Medium)]
        public string ContainerNo { get; set; }
        public string WeighingMachineCode { get; set; }
        public int? LeastCountDigitAfterDecimal { get; set; }
    }
}