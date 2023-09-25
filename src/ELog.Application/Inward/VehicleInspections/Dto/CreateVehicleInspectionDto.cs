using Abp.AutoMapper;

using ELog.Application.CommonService.Invoices.Dto;
using ELog.Application.Masters.InspectionChecklists.Dto;
using ELog.Core.Entities;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ELog.Application.Inward.VehicleInspections.Dto
{
    [AutoMapTo(typeof(VehicleInspectionHeader))]
    public class CreateVehicleInspectionDto
    {
        public int? GateEntryId { get; set; }
        public DateTime InspectionDate { get; set; }
        public int? TenantId { get; set; }

        [Required(ErrorMessage = "Checklist Type is required.")]
        public int ChecklistTypeId { get; set; }

        [Required(ErrorMessage = "Checklist is required.")]
        public int? InspectionChecklistId { get; set; }

        public int? TransactionStatusId { get; set; }

        public List<CheckpointDto> VehicleInspectionDetails { get; set; }

        public InvoiceDto InvoiceDto { get; set; }
    }
}