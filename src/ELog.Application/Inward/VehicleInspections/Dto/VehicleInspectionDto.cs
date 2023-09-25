using Abp.Application.Services.Dto;
using Abp.AutoMapper;

using ELog.Application.CommonService.Invoices.Dto;
using ELog.Application.Masters.InspectionChecklists.Dto;
using ELog.Core.Entities;

using System;
using System.Collections.Generic;

namespace ELog.Application.Inward.VehicleInspections.Dto
{
    [AutoMapFrom(typeof(VehicleInspectionHeader))]
    public class VehicleInspectionDto : EntityDto<int>
    {
        public int? GateEntryId { get; set; }

        public string GatePassNo { get; set; }

        public DateTime InspectionDate { get; set; }
        public int? TenantId { get; set; }

        public int ChecklistTypeId { get; set; }

        public int InspectionChecklistId { get; set; }

        public int? TransactionStatusId { get; set; }
        public List<CheckpointDto> VehicleInspectionDetails { get; set; }
        public InvoiceDto InvoiceDto { get; set; }
    }
}