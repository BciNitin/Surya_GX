using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using ELog.Core.Entities;
using System;

namespace ELog.Application.AreaCleaning.Dto
{
    [AutoMapFrom(typeof(AreaUsageLog))]
    public class AreaUsageLogListDto : EntityDto<int>
    {
        public int? ActivityID { get; set; }

        public int? CubicalId { get; set; }
        public string OperatorName { get; set; }

        public DateTime? StartTime { get; set; }

        public DateTime? StopTime { get; set; }

        public string Remarks { get; set; }

        public int? ApprovedBy { get; set; }
        public int? VerifiedBy { get; set; }
        public DateTime? ApprovedTime { get; set; }
        public int? StatusId { get; set; }


        public string UserEnteredActivityId { get; set; }
        public string UserEnteredCubicalId { get; set; }

        public bool IsActive { get; set; }
    }
}
