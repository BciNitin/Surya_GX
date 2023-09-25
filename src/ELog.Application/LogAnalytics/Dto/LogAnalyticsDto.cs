using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Elog.Core.Entities;
using System;

namespace ELog.Application.LogAnalytics.Dto
{
    [AutoMapFrom(typeof(ClientForm))]
    public class LogAnalyticsDto : EntityDto<int>
    {
        public int ClientId { get; set; }
        public string FormName { get; set; }
        public DateTime FormStartDate { get; set; }
        public DateTime FormEndDate { get; set; }
        public string FormJson { get; set; }
        public bool IsActive { get; set; }

        public DateTime CreationDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public int FormStatus { get; set; }
        public string UpdatedBy { get; set; }
        public string ApprovedBy { get; set; }
        public string CheckedBy { get; set; }
        public string CreatedBy { get; set; }
        public DateTime ApproveDateTime { get; set; }

    }

}


