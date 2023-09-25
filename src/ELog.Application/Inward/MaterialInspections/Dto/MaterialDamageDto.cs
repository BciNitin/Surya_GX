using Abp.Application.Services.Dto;
using Abp.AutoMapper;

using ELog.Core;
using ELog.Core.Entities;

using System.ComponentModel.DataAnnotations;

namespace ELog.Application.Inward.MaterialInspections.Dto
{
    [AutoMap(typeof(MaterialDamageDetail))]
    public class MaterialDamageDto : EntityDto<int>
    {
        public int? MaterialRelationId { get; set; }

        public int SequenceId { get; set; }

        [StringLength(PMMSConsts.Medium)]
        public string ContainerNo { get; set; }

        [StringLength(PMMSConsts.Medium)]
        public string Remark { get; set; }

        public float? Quantity { get; set; }

        public string QuantityInDecimal { get; set; }
        public int? UnitofMeasurementId { get; set; }
    }
}