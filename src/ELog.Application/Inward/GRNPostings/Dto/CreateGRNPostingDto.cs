using Abp.AutoMapper;

using ELog.Core;
using ELog.Core.Entities;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ELog.Application.Inward.VehicleInspections.Dto
{
    [AutoMapTo(typeof(GRNHeader))]
    public class CreateGRNPostingDto
    {
        [StringLength(PMMSConsts.Small)]
        public string GRNPostingNumber { get; set; }

        public int? PurchaseOrderId { get; set; }

        public int? TenantId { get; set; }
        public DateTime GRNPostingDate { get; set; }

        public List<GRNPostingDetailsDto> GRNDetails { get; set; }
    }
}