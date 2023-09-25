using Abp.Application.Services.Dto;
using System;

namespace ELog.Application.Dispensing.Destruction.Dto
{
    public class DestructionDto : EntityDto<int>
    {
        public string MovementType { get; set; }
        public int ContainerId { get; set; }
        public string MaterialContainerBarCode { get; set; }
        public string MaterialCode { get; set; }
        public string SAPBatchNo { get; set; }
        public string ARNo { get; set; }
        public float? Quantity { get; set; }
        public string UnitOfMeasurement { get; set; }
        public Guid? TransactionId { get; set; }
        public bool? IsSAPPosted { get; set; }
    }
}