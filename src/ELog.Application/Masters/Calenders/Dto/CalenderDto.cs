using Abp.Application.Services.Dto;
using Abp.AutoMapper;

using ELog.Core;
using ELog.Core.Entities;

using System;
using System.ComponentModel.DataAnnotations;

namespace ELog.Application.Masters.Calenders.Dto
{
    [AutoMapFrom(typeof(CalenderMaster))]
    public class CalenderDto : EntityDto<int>
    {
        public int SubPlantId { get; set; }
        public DateTime CalenderDate { get; set; }

        public int HolidayTypeId { get; set; }

        [StringLength(PMMSConsts.Medium)]
        public string HolidayName { get; set; }

        public string Description { get; set; }
        public bool IsActive { get; set; }

        public bool IsApprovalRequired { get; set; }
        public string UserEnteredApprovalStatus { get; set; }
        public string ApprovalStatusDescription { get; set; }
    }
}