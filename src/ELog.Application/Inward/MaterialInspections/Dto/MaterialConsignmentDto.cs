using Abp.Application.Services.Dto;
using Abp.AutoMapper;

using ELog.Core;
using ELog.Core.Entities;

using System;
using System.ComponentModel.DataAnnotations;

namespace ELog.Application.Inward.MaterialInspections.Dto
{
    [AutoMap(typeof(MaterialConsignmentDetail))]
    public class MaterialConsignmentDto : EntityDto<int>
    {
        [StringLength(PMMSConsts.Medium)]
        public string ManufacturedBatchNo { get; set; }

        public DateTime? ManufacturedDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public DateTime? RetestDate { get; set; }
        public float? QtyAsPerInvoice { get; set; }
        public string QtyAsPerInvoiceInDecimal { get; set; }
        public int? UnitofMeasurementId { get; set; }
        public int? SequenceId { get; set; }
        public int? MaterialRelationId { get; set; }

        public int RemainingDaysLeftForExpiry { get; set; }
        public int RemainingDaysLeftForRetest { get; set; }

        public string StatusForExpiry { get; set; }
        public string StatusForRetest { get; set; }
    }
}