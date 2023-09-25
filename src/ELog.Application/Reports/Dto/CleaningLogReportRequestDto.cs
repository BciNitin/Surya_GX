using Abp.Application.Services.Dto;

using System;
using System.Collections.Generic;
using System.Text;

namespace ELog.Application.Reports.Dto
{
   public class CleaningLogReportRequestDto:PagedAndSortedResultRequestDto
    {
        public int? SubPlantId { get; set; }
        public List<int> EquipmentListIds { get; set; }
        public List<int> CubicleListIds { get; set; }
        public List<int> cleaningTypeIds { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public bool? IsSampling { get; set; }
    }
}
