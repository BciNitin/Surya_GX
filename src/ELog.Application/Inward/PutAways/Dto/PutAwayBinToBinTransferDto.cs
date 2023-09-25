using Abp.Application.Services.Dto;
using Abp.AutoMapper;

using ELog.Core.Entities;

using System;

namespace ELog.Application.Inward.PutAways.Dto
{
    [AutoMapTo(typeof(PutAwayBinToBinTransfer))]
    public class PutAwayBinToBinTransferDto : EntityDto<int>
    {
        public Guid TransactionId { get; set; }
        public bool IsUnloaded { get; set; }
        public int MaterialTransferTypeId { get; set; }
        public string LocationBarcode { get; set; }
        public string PalletBarcode { get; set; }
        public string MaterialDescription { get; set; }
        public int? PalletId { get; set; }
        public int? LocationId { get; set; }
        public int? MaterialId { get; set; }
        public string SAPBatchNumber { get; set; }
        public int ContainerNo { get; set; }
        public int? ContainerId { get; set; }
        public int? PutAwayHeaderId { get; set; }

    }
}