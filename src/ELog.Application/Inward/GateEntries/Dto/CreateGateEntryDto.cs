using Abp.AutoMapper;

using ELog.Application.CommonService.Invoices.Dto;
using ELog.Application.SAP.PurchaseOrder.Dto;
using ELog.Core;
using ELog.Core.Entities;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ELog.Application.Inward.GateEntries.Dto
{
    [AutoMapTo(typeof(GateEntry))]
    public class CreateGateEntryDto
    {
        [StringLength(PMMSConsts.Small)]
        public string GatePassNo { get; set; }

        public int PrintCount { get; set; }

        public bool IsActive { get; set; }
        public int? PrinterId { get; set; }
        public CreateInvoiceDto InvoiceDto { get; set; }
        public List<MaterialInternalDto> Materials { get; set; }
    }
}