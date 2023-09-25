using Abp.AutoMapper;

using ELog.Core;
using ELog.Core.Entities;

using System;
using System.ComponentModel.DataAnnotations;

namespace ELog.Application.Masters.Calenders.Dto
{
    [AutoMapTo(typeof(CalenderMaster))]
    public class CreateCalenderDto
    {
        public int SubPlantId { get; set; }
        public DateTime CalenderDate { get; set; }

        [StringLength(PMMSConsts.Medium)]
        public string HolidayName { get; set; }

        public int HolidayTypeId { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
    }
}