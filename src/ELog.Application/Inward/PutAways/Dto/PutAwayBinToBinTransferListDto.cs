using Abp.Application.Services.Dto;
using Abp.AutoMapper;

using ELog.Core.Entities;

using System;

namespace ELog.Application.Inward.PutAways.Dto
{
    [AutoMapFrom(typeof(PutAwayBinToBinTransfer))]
    public class PutAwayBinToBinTransferListDto : EntityDto<int>
    {
        public int? MaterialTransferTypeId { get; set; }
        public string PalletBarcode { get; set; }
        public string LocationBarCode { get; set; }
        public int Count { get; set; }
        public int? LocationId { get; set; }
        public string MaterialCode { get; set; }
        public Guid TransactionId { get; set; }
        public int? PalletId { get; set; }
        public int? MaterialId { get; set; }
        public int PlantId { get; set; }
        public string ItemDescription { get; set; }
        public string SAPBatchNumber { get; set; }
    }
}