using Abp.Application.Services.Dto;

using ELog.Core;

using System;
using System.ComponentModel.DataAnnotations;

namespace ELog.Application.SAP.ProcessOrder.Dto
{
    public class ProcessOrderMaterialInternalDto : EntityDto<int>
    {
        [StringLength(PMMSConsts.Medium)]
        [Required]
        public string ProcessOrderNo { get; set; }

        [Required]
        [StringLength(PMMSConsts.Medium)]
        public string ItemNo { get; set; }

        [Required]
        [StringLength(PMMSConsts.Medium)]
        public string ItemCode { get; set; }

        public string ItemDescription { get; set; }

        public float? OrderQuantity { get; set; }

        public int? UnitOfMeasurementId { get; set; }
        public string UnitOfMeasurement { get; set; }

        public int? TenantId { get; set; }

        [StringLength(PMMSConsts.Medium)]
        public string BatchNo { get; set; }

        [StringLength(PMMSConsts.Medium)]
        public string SAPBatchNo { get; set; }

        public DateTime ExpiryDate { get; set; }

        public DateTime RetestDate { get; set; }

        [StringLength(PMMSConsts.Medium)]
        public string ARNo { get; set; }
    }
}