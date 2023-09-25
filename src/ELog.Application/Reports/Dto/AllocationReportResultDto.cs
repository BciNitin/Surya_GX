using Abp.Application.Services.Dto;
using Abp.AutoMapper;

using ELog.Core.Entities;

using System;

namespace ELog.Application.Reports.Dto
{
    [AutoMapFrom(typeof(PutAwayBinToBinTransfer))]
    public class AllocationReportResultDto : EntityDto<int>
    {
        public DateTime AllocationDate { get; set; }
        public string PlantCode { get; set; }
        public string AreaCode { get; set; }
        public string LocationCode { get; set; }
        public string PalletCode { get; set; }
        public string MaterialCode { get; set; }
        public string SapBatchNo { get; set; }
        public float? BalQty { get; set; }
        public float? Qty { get; set; }
        public string ContainerBarcode { get; set; }
        public string StockStatus { get; set; }
        public int? SubPlantId { get; set; }
        public int? AreaId { get; set; }
        public int? LocationId { get; set; }
        public int? PalletId { get; set; }
        public int? MaterialId { get; set; }
        public string AllocatedBy { get; set; }
    }
}