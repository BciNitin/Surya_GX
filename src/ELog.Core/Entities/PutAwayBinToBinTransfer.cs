using ELog.Core.Authorization;

using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ELog.Core.Entities
{
    [Table("PutAwayBinToBinTransfer")]
    public class PutAwayBinToBinTransfer : PMMSFullAudit
    {
        [ForeignKey("LocationId")]
        public int? LocationId { get; set; }
        [ForeignKey("PalletId")]
        public int? PalletId { get; set; }
        [ForeignKey("MaterialId")]
        public int? MaterialId { get; set; }
        [ForeignKey("ContainerId")]
        public int? ContainerId { get; set; }
        [ForeignKey("MaterialTransferTypeId")]
        public int? MaterialTransferTypeId { get; set; }
        public Guid TransactionId { get; set; }
        public string SAPBatchNumber { get; set; }
        public int ContainerNo { get; set; }
        public bool IsUnloaded { get; set; }
        public int? TenantId { get; set; }
    }
}