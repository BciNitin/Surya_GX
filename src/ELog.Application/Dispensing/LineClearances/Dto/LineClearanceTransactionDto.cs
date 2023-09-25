using Abp.Application.Services.Dto;
using Abp.AutoMapper;

using ELog.Application.Masters.InspectionChecklists.Dto;
using ELog.Core.Entities;

using System;
using System.Collections.Generic;

namespace ELog.Application.Dispensing.LineClearances.Dto
{
    [AutoMapTo(typeof(LineClearanceTransaction))]
    public class LineClearanceTransactionDto : EntityDto<int>
    {
        public DateTime ClearanceDate { get; set; }
        public int CubicleId { get; set; }
        public int AreaId { get; set; }
        public int GroupId { get; set; }
        public int GroupName { get; set; }
        public int StatusId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? StopTime { get; set; }
        public DateTime? ApprovedTime { get; set; }
        public int? ApprovedBy { get; set; }
        public int? VerifiedBy { get; set; }
        public List<CheckpointDto> LineClearanceCheckpoints { get; set; }
        public bool IsVerified { get; set; }
        public bool IsApproved { get; set; }

        public bool CanApproved { get; set; }
        public bool CanVerified { get; set; }
        public string ApprovedByName { get; set; }
        public string CreatorName { get; set; }
        public bool IsInValidTransaction { get; set; }
    }
}