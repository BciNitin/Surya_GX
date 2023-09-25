using Abp.Application.Services.Dto;

using System;
using System.Collections.Generic;

namespace PMMS.Application.ElogApi.Dto
{
    public class InsertLogWiseDataDto : PagedAndSortedResultRequestDto
    {
        public string TableName { get; set; }
        public string ColumnName { get; set; }

        public string Values { get; set; }
        public string ActionType { get; set; }
        

    }
}