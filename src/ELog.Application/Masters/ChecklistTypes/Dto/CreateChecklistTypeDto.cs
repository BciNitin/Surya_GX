using Abp.AutoMapper;

using ELog.Core;
using ELog.Core.Entities;

using System.ComponentModel.DataAnnotations;

namespace ELog.Application.Masters.ChecklistTypes.Dto
{
    [AutoMapTo(typeof(ChecklistTypeMaster))]
    public class CreateChecklistTypeDto
    {
        [StringLength(PMMSConsts.Medium)]
        public string ChecklistName { get; set; }

        public int? SubPlantId { get; set; }

        public int? SubModuleId { get; set; }

        public bool IsActive { get; set; }
    }
}