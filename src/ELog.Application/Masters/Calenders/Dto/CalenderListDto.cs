using Abp.Application.Services.Dto;
using Abp.AutoMapper;

using ELog.Core.Entities;

using System;

namespace ELog.Application.Masters.Calenders.Dto
{
    [AutoMapFrom(typeof(CalenderMaster))]
    public class CalenderListDto : EntityDto<int>
    {
        public int SubPlantId { get; set; }
        public DateTime CalenderDate { get; set; }

        public int HolidayTypeId { get; set; }
        public int ApprovalStatusId { get; set; }
        public string HolidayName { get; set; }
        public string Description { get; set; }
        public string UserEnteredSubPlantId { get; set; }
        public bool IsActive { get; set; }
        public string UserEnteredHolidayType { get; set; }
        public string UserEnteredApprovalStatus { get; set; }
    }
}