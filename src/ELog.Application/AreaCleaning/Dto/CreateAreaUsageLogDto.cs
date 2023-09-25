using Abp.AutoMapper;
using ELog.Application.Masters.InspectionChecklists.Dto;
using ELog.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ELog.Application.AreaCleaning.Dto
{
    [AutoMapTo(typeof(AreaUsageLog))]
    public class CreateAreaUsageLogDto
    {
        [Required(ErrorMessage = "Activity is required.")]
        public int ActivityID { get; set; }

        [Required(ErrorMessage = "Cubical is required.")]
        public int CubicalId { get; set; }
        public string CubicalCode { get; set; }

        [Required(ErrorMessage = "Operator Name  is required.")]
        public string OperatorName { get; set; }


        public DateTime? StartTime { get; set; }

        public DateTime? StopTime { get; set; }

        public string Remarks { get; set; }
        public int? ApprovedBy { get; set; }
        public int? VerifiedBy { get; set; }
        public DateTime? ApprovedTime { get; set; }
        public int? StatusId { get; set; }


        public bool IsActive { get; set; }

        public List<CheckpointDto> AreaUsageLogLists { get; set; }
    }
}
