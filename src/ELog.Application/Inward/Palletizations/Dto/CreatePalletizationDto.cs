using Abp.AutoMapper;

using ELog.Core.Entities;

using System;
using System.ComponentModel.DataAnnotations;

namespace ELog.Application.Inward.Palletizations.Dto
{
    [AutoMapTo(typeof(Palletization))]
    public class CreatePalletizationDto
    {
        [Required(ErrorMessage = "Pallet Barcode  is required.")]
        public int? PalletId { get; set; }

        [Required(ErrorMessage = "Material Barcode is required.")]
        public int? MaterialId { get; set; }

        public int ContainerNo { get; set; }
        public string ContainerBarCode { get; set; }
        public Guid TransactionId { get; set; }
        public int PartitionId { get; set; }
        public bool IsUnloaded { get; set; }
        public string PalletBarcode { get; set; }
        public string SAPBatchNumber { get; set; }
        public int? GRNDetailId { get; set; }
        public int? ContainerId { get; set; }
    }
}