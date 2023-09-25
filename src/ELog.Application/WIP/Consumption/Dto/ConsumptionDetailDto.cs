using Abp.Application.Services.Dto;
using Abp.AutoMapper;

using ELog.Core;
using ELog.Core.Entities;
using System.ComponentModel.DataAnnotations;

namespace ELog.Application.WIP.Consumption.Dto
{
    [AutoMapFrom(typeof(ConsumptionDetails))]
    public class ConsumptionDetailDto : EntityDto<int>
    {
        public int? ConsumptionId { get; set; }
        public int? MaterialBarocdeId { get; set; }

        //public int? BatchNo { get; set; }

        public string ProductCode { get; set; }
        public string LineItemNo { get; set; }

        public int? ProcessOrderMaterialId { get; set; }

        public string MaterialCode { get; set; }

        [StringLength(PMMSConsts.Large)]
        public string MaterialDescription { get; set; }

        [StringLength(PMMSConsts.Small)]
        public string BatchNo { get; set; }

        [StringLength(PMMSConsts.Small)]
        public string SAPBatchNumber { get; set; }

        public float? Qty { get; set; }

        [StringLength(PMMSConsts.Small)]
        public string UnitOfMeasurement { get; set; }

        public int? UnitOfMeasurementId { get; set; }

        public string ExpiryDate { get; set; }
        public string RetestDate { get; set; }

        [StringLength(PMMSConsts.Small)]
        public string ARNo { get; set; }
    }
}
