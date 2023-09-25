using Abp.AutoMapper;
using ELog.Core.Entities;
using System;

namespace ELog.Application.AreaCleaning.Dto
{
    [AutoMapTo(typeof(AreaUsageListLog))]
    public class CreateAreaUsageListLog
    {
        public int AreaUsageId { get; set; }
        public int InsCheckListId { get; set; }
        public string Status { get; set; }
        public string Remarks { get; set; }
        public int? ApprovedBy { get; set; }
        public int? VerifiedBy { get; set; }
        public DateTime? ApprovedTime { get; set; }
        public int? StatusId { get; set; }

        public bool IsActive { get; set; }
    }
}
