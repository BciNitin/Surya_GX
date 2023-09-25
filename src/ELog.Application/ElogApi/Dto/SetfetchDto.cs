using Abp.Application.Services.Dto;

using System;
using System.Collections.Generic;
using System.Data;

namespace PMMS.Application.ElogApi.Dto
{
    public class SetfetchDto : PagedAndSortedResultRequestDto
    {
        public string tablE_CATALOG { get; set; }
        public string tablE_SCHEMA { get; set; }
        public string tablE_NAME { get; set; }
        public string tablE_TYPE { get; set; }
        public DataTable allData { get; set; }
     
    }
}