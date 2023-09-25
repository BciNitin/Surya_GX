using Abp.Application.Services.Dto;
using Abp.AutoMapper;

using ELog.Core.Entities;

using System;

namespace ELog.Application.Dispensing.EquipmentCleanings.Dto
{
    [AutoMapTo(typeof(EquipmentCleaningStatus))]
    public class EquipmentCleaningStatusDto : EntityDto<int>
    {
        public DateTime CleaningDate { get; set; }
        public int? EquipmentId { get; set; }
        public int StatusId { get; set; }
        public bool IsStartButtonVisible { get; set; }
        public int? EquipmentCleaningTransactionId { get; set; }
    }
}