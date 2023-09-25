using Abp.Application.Services.Dto;
using Abp.AutoMapper;

using ELog.Application.Masters.InspectionChecklists.Dto;
using ELog.Core.Entities;

using System.Collections.Generic;

namespace ELog.Application.Inward.VehicleInspections.Dto
{
    [AutoMapFrom(typeof(VehicleInspectionHeader))]
    public class UpdateVehicleInspectionDto : EntityDto<int>
    {
        public int? TransactionStatusId { get; set; }

        public List<CheckpointDto> VehicleInspectionDetails { get; set; }
    }
}