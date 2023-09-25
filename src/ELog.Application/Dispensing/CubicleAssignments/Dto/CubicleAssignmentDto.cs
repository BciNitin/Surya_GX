using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using ELog.Core;
using ELog.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ELog.Application.Dispensing.CubicleAssignments.Dto
{
    [AutoMapFrom(typeof(CubicleAssignmentHeader))]
    public class CubicleAssignmentDto : EntityDto<int>
    {
        [StringLength(PMMSConsts.Small)]
        public string GroupId { get; set; }

        [StringLength(PMMSConsts.Small)]
        public string ProductCode { get; set; }

        public int? TenantId { get; set; }
        public DateTime CubicleAssignmentDate { get; set; }
        public bool IsSampling { get; set; }

        [Required]
        public List<CubicleAssignmentDetailsDto> CubicleAssignmentDetails { get; set; }
    }
}