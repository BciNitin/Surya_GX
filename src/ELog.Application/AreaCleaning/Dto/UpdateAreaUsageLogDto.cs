using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using ELog.Application.Masters.InspectionChecklists.Dto;
using System;
using System.Collections.Generic;

namespace ELog.Application.AreaCleaning.Dto
{
    [AutoMapFrom(typeof(ELog.Core.Entities.AreaUsageLog))]
    public class UpdateAreaUsageLogDto : EntityDto<int>
    {
        public DateTime? StopTime { get; set; }

        public string Remarks { get; set; }
        public int? ApprovedBy { get; set; }
        public int? VerifiedBy { get; set; }
        public DateTime? ApprovedTime { get; set; }
        public int? StatusId { get; set; }

        public bool IsApproved { get; set; }
        public bool IsRejected { get; set; }

        public List<CheckpointDto> AreaUsageLogLists { get; set; }
    }
}
