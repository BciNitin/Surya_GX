using Abp.Application.Services.Dto;

using System;
using System.Collections.Generic;

namespace ELog.Application.Reports.Dto
{
    public class AuditDynamicReportDto : PagedAndSortedResultRequestDto
    {
        public string? tableName { get; set; }
        public string? param { get; set; }

    }
}