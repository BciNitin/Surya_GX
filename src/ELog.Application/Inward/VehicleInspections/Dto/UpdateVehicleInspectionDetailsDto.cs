using Abp.Application.Services.Dto;
using Abp.AutoMapper;

using ELog.Core;
using ELog.Core.Entities;

using System.ComponentModel.DataAnnotations;

namespace ELog.Application.Inward.VehicleInspections.Dto
{
    [AutoMapFrom(typeof(VehicleInspectionDetail))]
    public class UpdateVehicleInspectionDetailDto : EntityDto<int>
    {
        public int? VehicleInspectionHeaderId { get; set; }

        public int CheckpointId { get; set; }
        public int? TenantId { get; set; }

        [StringLength(PMMSConsts.Large)]
        public string DiscrepancyRemark { get; set; }

        [StringLength(PMMSConsts.Large)]
        public string Observation { get; set; }
    }
}