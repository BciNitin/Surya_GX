using Abp.AutoMapper;
using ELog.Core;
using ELog.Core.Entities;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ELog.Application.Dispensing.CubicleAssignments.Dto
{
    [AutoMapTo(typeof(CubicleAssignmentHeader))]
    public class CreateCubicleAssignmentDto
    {
        [StringLength(PMMSConsts.Small)]
        public string GroupId { get; set; }

        //  [Required]
        [StringLength(PMMSConsts.Small)]
        public string ProductCode { get; set; }

        public int? TenantId { get; set; }
        public bool IsSampling { get; set; }

        [Required]
        public DateTime CubicleAssignmentDate { get; set; }

        public List<CubicleAssignmentDetailsDto> CubicleAssignmentDetails { get; set; }
    }
}