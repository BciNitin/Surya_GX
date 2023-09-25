using Abp.Application.Services.Dto;
using Abp.AutoMapper;

using ELog.Core.Entities;

namespace ELog.Application.WIP.WeightVerification.Dto
{
    [AutoMapFrom(typeof(WeightVerificationHeader))]
    public class WeightVerificationDto : EntityDto<int>
    {
        public int? ProductId { get; set; }
        public string ProductCode { get; set; }

        public string ProductName { get; set; }
        public int? BatchId { get; set; }
        public string Batchno { get; set; }
        public int? LotId { get; set; }
        public string Lotno { get; set; }
        public int? ProcessOrderId { get; set; }
        public int? CubicalId { get; set; }
        public int? DispensedId { get; set; }
        public int? UnitofMeasurementId { get; set; }

        public int? NoOfContainers { get; set; }
        public int? NoOfPacks { get; set; }
        public int? RecivedNoOfPacks { get; set; }
        public int ScanBalanceId { get; set; }
        public string ScanBalanceNo { get; set; }
        public float? DispGrossWeight { get; set; }
        public float? NetWeight { get; set; }
        public float? TareWeight { get; set; }
        public float? GrossWeight { get; set; }
        public bool IsGrossWeight { get; set; }

        public bool IsSuccess { get; set; }
        public string UOM { get; set; }
        public string MatCode { get; set; }
        public string MatDesc { get; set; }





    }
}
