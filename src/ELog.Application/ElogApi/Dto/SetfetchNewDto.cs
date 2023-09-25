using Abp.Application.Services.Dto;

using System;
using System.Collections.Generic;
using System.Data;

namespace PMMS.Application.ElogApi.Dto
{
    public class SetfetchNewDto : PagedAndSortedResultRequestDto
    {
        public DataTable columnName { get; set; }
      
    }
}