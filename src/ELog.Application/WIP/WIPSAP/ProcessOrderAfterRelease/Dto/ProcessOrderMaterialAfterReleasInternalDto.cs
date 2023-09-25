using Abp.Application.Services.Dto;

using ELog.Core;

using System;
using System.ComponentModel.DataAnnotations;

namespace ELog.Application.WIP.WIPSAP.ProcessOrderAfterRelease.Dto
{
    public class ProcessOrderMaterialAfterReleasInternalDto : EntityDto<int>
    {
        public int ProcessOrderId { get; set; }

        [StringLength(PMMSConsts.Medium)]
        [Required]
        public string ProcessOrderNo { get; set; }

        [Required]
        [StringLength(PMMSConsts.Medium)]
        public string LineItemNo { get; set; }

        [Required]
        [StringLength(PMMSConsts.Medium)]
        public string MaterialCode { get; set; }

        public string MaterialDescription { get; set; }

        [StringLength(PMMSConsts.Medium)]
        public string ARNo { get; set; }

        [StringLength(PMMSConsts.Medium)]
        public string SAPBatchNo { get; set; }

        [StringLength(PMMSConsts.Medium)]
        public string ProductBatchNo { get; set; }

        [StringLength(PMMSConsts.Medium)]
        public string CurrentStage { get; set; }

        [StringLength(PMMSConsts.Medium)]
        public string NextStage { get; set; }

        public float Quantity { get; set; }
        public string UnitOfMeasurement { get; set; }

        public DateTime ExpiryDate { get; set; }

        public DateTime RetestDate { get; set; }
    }
}
