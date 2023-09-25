using Abp.Application.Services.Dto;
using Abp.AutoMapper;

using ELog.Core.Entities;

using System;

namespace ELog.Application.Dispensing.CubicleCleanings.Dto
{
    [AutoMapTo(typeof(CubicleCleaningDailyStatus))]
    public class CubicleCleaningDailyStatusDto : EntityDto<int>
    {
        public DateTime CleaningDate { get; set; }
        public int? CubicleId { get; set; }
        public int StatusId { get; set; }
        public bool isStartButtonVisible { get; set; }
        public int? CubicleCleaningTransactionId { get; set; }
    }
}