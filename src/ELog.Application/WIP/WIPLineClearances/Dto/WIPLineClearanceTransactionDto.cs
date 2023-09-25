using Abp.Application.Services.Dto;
using Abp.AutoMapper;

using ELog.Application.Masters.InspectionChecklists.Dto;
using ELog.Core.Entities;

using System;
using System.Collections.Generic;

namespace ELog.Application.WIP.WIPLineClearances.Dto
{
    [AutoMapTo(typeof(WIPLineClearanceTransaction))]
    public class WIPLineClearanceTransactionDto : EntityDto<int>
    {
        public DateTime ClearanceDate { get; set; }
        public int? ProductId { get; set; }
        public string ProductCode { get; set; }
        public int? ProcessOrderId { get; set; }

        public string ProcessOrderNo { get; set; }
        public int CubicleBarcodeId { get; set; }

        public string CubicleBarcode { get; set; }

        public int EquipmentBarcodeId { get; set; }

        public string EquipmentBarcode { get; set; }
        public int StatusId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? StopTime { get; set; }
        public DateTime? ApprovedTime { get; set; }
        public int? ApprovedBy { get; set; }
        public int? VerifiedBy { get; set; }
        public List<CheckpointDto> LineClearanceCheckpoints { get; set; }
        public bool IsVerified { get; set; }
        public bool IsApproved { get; set; }

        public bool CanApproved { get; set; }
        public bool CanVerified { get; set; }
        public string ApprovedByName { get; set; }
        public string CreatorName { get; set; }
        public bool IsInValidTransaction { get; set; }
        public string BatchNo { get; set; }
        public int? ChecklistTypeId { get; set; }
        public int? InspectionChecklistId { get; set; }
        public string Remarks { get; set; }
    }
}
