using Abp.Application.Services.Dto;
using Abp.AutoMapper;

using ELog.Application.Masters.InspectionChecklists.Dto;
using ELog.Core.Entities;

using System;
using System.Collections.Generic;

namespace ELog.Application.WIP.WIPLineClearances.Dto
{
    [AutoMapTo(typeof(WIPLineClearanceTransaction))]
    public class CreateWIPLineClearanceDto : EntityDto<int>
    {
        public DateTime LineClearanceDate { get; set; }
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
        public int? ApprovedBy { get; set; }
        public int? VerifierBy { get; set; }
        public List<CheckpointDto> LineClearanceCheckpoints { get; set; }
        public bool IsVerified { get; set; }
        public bool CanApproved { get; set; }
        public int? ChecklistTypeId { get; set; }
        public string Remarks { get; set; }
    }
}
