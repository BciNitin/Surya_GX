using Abp.Application.Services.Dto;

using ELog.Application.CommonService.Invoices.Dto;
using ELog.Application.Masters.InspectionChecklists.Dto;

using System.Collections.Generic;

namespace ELog.Application.Inward.MaterialInspections.Dto
{
    public class MaterialInspectionDto : EntityDto<int>
    {
        public int? GateEntryId { get; set; }
        public string GatePassNo { get; set; }
        public int? MaterialId { get; set; }
        public string MaterialCode { get; set; }
        public string MaterialDescription { get; set; }
        public int? ChecklistTypeId { get; set; }
        public int? InspectionChecklistId { get; set; }
        public int? MaterialInspectionTransactionId { get; set; }
        public int? MaterialTransactionId { get; set; }
        public InvoiceDto InvoiceDetails { get; set; }
        public int? MaterialRelationId { get; set; }
        public List<MaterialConsignmentDto> MaterialConsignments { get; set; }
        public List<CheckpointDto> MaterialCheckpoints { get; set; }
        public List<MaterialDamageDto> MaterialDamageDetails { get; set; }
    }
}