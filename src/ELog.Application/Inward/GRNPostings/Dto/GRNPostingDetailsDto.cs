using Abp.Application.Services.Dto;
using Abp.AutoMapper;

using ELog.Core;
using ELog.Core.Entities;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ELog.Application.Inward.VehicleInspections.Dto
{
    [AutoMapFrom(typeof(GRNDetail))]
    public class GRNPostingDetailsDto : EntityDto<int>
    {
        public int? GRNHeaderId { get; set; }

        [StringLength(PMMSConsts.Small)]
        public string SAPBatchNumber { get; set; }

        public int? TenantId { get; set; }
        public int? FormArrayIndex { get; set; }

        public bool IsSelected { get; set; }
        public string InvoiceNo { get; set; }
        public int? InvoiceId { get; set; }

        public int? MaterialId { get; set; }

        public int? ParentRow { get; set; }
        public string MaterialCode { get; set; }
        public string ItemCode { get; set; }

        public int? MfgBatchNoId { get; set; }

        [StringLength(PMMSConsts.Medium)]
        public string ManufacturedBatchNo { get; set; }

        public float? ConsignmentQty { get; set; }
        public string ConsignmentQtyUnit { get; set; }

        public List<GRNPostingQtyDetailsDto> GRNQtyDetails { get; set; }

        public float? TotalQty { get; set; }

        public float? NoOfContainer { get; set; }

        public float? QtyPerContainer { get; set; }


        public string TotalQtyInDecimal { get; set; }

        public string QtyPerContainerInDecimal { get; set; }

        [StringLength(PMMSConsts.Large)]
        public string DiscrepancyRemark { get; set; }
        public string LineItem { get; set; }
        public string ItemDescription { get; set; }
        public string GRNPreparedBy { get; set; }
        public float? InvoiceQty { get; set; }
        public int MaterialConsignmentId { get; set; }
        public int? MaterialRelationId { get; set; }
        public string IsDamaged { get; set; }
        public string UOM { get; set; }
    }
}