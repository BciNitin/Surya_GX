using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using ELog.Application.Masters.InspectionChecklists.Dto;
using System;
using System.Collections.Generic;

namespace ELog.Application.EquipmentUsageLog.Dto
{
    [AutoMapFrom(typeof(ELog.Core.Entities.EquipmentUsageLog))]
    public class UpdateEquipmentUsageLogDto : EntityDto<int>
    {
        public DateTime? EndTime { get; set; }

        public string Remarks { get; set; }

        public int? ApprovedBy { get; set; }
        public DateTime? ApprovedTime { get; set; }
        public int? StatusId { get; set; }

        public bool IsApproved { get; set; }
        public bool IsRejected { get; set; }

        public List<CheckpointDto> EquipmentUsageLogLists { get; set; }
    }
}
