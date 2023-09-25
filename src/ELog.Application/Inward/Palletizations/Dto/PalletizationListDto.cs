using Abp.Application.Services.Dto;
using Abp.AutoMapper;

using ELog.Core.Entities;

using System;

namespace ELog.Application.Inward.Palletizations.Dto
{
    [AutoMapFrom(typeof(Palletization))]
    public class PalletizationListDto : EntityDto<string>
    {
        public string PalletBarcode { get; set; }
        public string MaterialCode { get; set; }
        public string MaterialDescription { get; set; }
        public string SAPBatchNumber { get; set; }
        public int Count { get; set; }
        public Guid TransactionId { get; set; }
        public int? PalletId { get; set; }
        public int? MaterialId { get; set; }
        public int PlantId { get; set; }
    }
}