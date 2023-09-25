using Abp.Application.Services.Dto;
using Abp.AutoMapper;

using ELog.Application.CommonService.Invoices.Dto;
using ELog.Application.SAP.PurchaseOrder.Dto;
using ELog.Core;
using ELog.Core.Entities;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ELog.Application.Inward.GateEntries.Dto
{
    [AutoMapFrom(typeof(GateEntry))]
    public class GateEntryDto : EntityDto<int>
    {
        [StringLength(PMMSConsts.Small)]
        public string GatePassNo { get; set; }

        public int? TenantId { get; set; }

        [Required]
        public List<MaterialInternalDto> Materials { get; set; }

        public int PrintCount { get; set; }
        public bool IsActive { get; set; }
        public int? PrinterId { get; set; }
        public InvoiceDto InvoiceDto { get; set; }
        public int? PlantId { get; set; }
        public string PurchaseOrderDeliverSchedule { get; set; }
        public int? TransactionstatusId { get; set; }

    }
}