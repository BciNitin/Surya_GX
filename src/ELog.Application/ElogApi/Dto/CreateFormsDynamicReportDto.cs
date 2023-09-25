using System;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using System.Text;

namespace PMMS.Application.ElogApi.Dto
{
   public class CreateFormsDynamicReportDto: PagedAndSortedResultRequestDto
    {
        public string? tableName { get; set; }
        public int? pageNumber { get; set; }
        public int? pageSize { get; set; }

    }
}
