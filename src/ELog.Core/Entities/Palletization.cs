using ELog.Core.Authorization;

using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ELog.Core.Entities
{
    [Table("Palletizations")]
    public class Palletization : PMMSFullAudit
    {
        [ForeignKey("PalletId")]
        public int? PalletId { get; set; }

        [ForeignKey("MaterialId")]
        public int? MaterialId { get; set; }

        [ForeignKey("GRNDetailId")]
        public int? GRNDetailId { get; set; }

        [ForeignKey("ContainerId")]
        public int? ContainerId { get; set; }

        public Guid TransactionId { get; set; }
        public string SAPBatchNumber { get; set; }
        public int ContainerNo { get; set; }
        public string ContainerBarCode { get; set; }
        public bool IsUnloaded { get; set; }
        public int? TenantId { get; set; }
        public string ProductBatchNo { get; set; }
    }
}