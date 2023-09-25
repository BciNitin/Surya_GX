using Abp.Application.Services.Dto;
using Abp.AutoMapper;

using ELog.Core.Entities;

using System;
using System.Collections.Generic;
using System.Text;

namespace ELog.Application.Reports.Dto
{
    [AutoMapFrom(typeof(ReportConfiguration))]
    public class ReportConfigurationDto : EntityDto<int>
    {
        public long? UserId { get; set; }
        public int SubModuleId { get; set; }
        public string ReportSettings { get; set; }
        public string SubModuleName { get; set; }
    }
}
