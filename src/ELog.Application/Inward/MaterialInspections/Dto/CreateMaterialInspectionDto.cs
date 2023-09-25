using ELog.Application.CommonService.Invoices.Dto;
using ELog.Application.Masters.InspectionChecklists.Dto;

using System.Collections.Generic;

namespace ELog.Application.Inward.MaterialInspections.Dto
{
    public class CreateMaterialInspectionDto
    {
        public int? GateEntryId { get; set; }
        public string GatePassNo { get; set; }
        public int? MaterialId { get; set; }
        public string MaterialDescription { get; set; }
        public int? MaterialHeaderId { get; set; }

        public int? ChecklistTypeId { get; set; }
        public int? InspectionChecklistId { get; set; }
        public int? MaterialInspectionTransactionId { get; set; }
        public int? MaterialTransactionId { get; set; }
        public InvoiceDto InvoiceDetails { get; set; }
        public List<MaterialConsignmentDto> MaterialConsignments { get; set; }
        public List<CheckpointDto> MaterialCheckpoints { get; set; }
        public List<MaterialDamageDto> MaterialDamageDetails { get; set; }
        public int? TransactionStatusId { get; set; }
    }
}