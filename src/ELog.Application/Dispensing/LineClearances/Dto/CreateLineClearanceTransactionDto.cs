using Abp.Application.Services.Dto;
using Abp.AutoMapper;

using ELog.Application.Masters.InspectionChecklists.Dto;
using ELog.Core.Entities;

using System;
using System.Collections.Generic;

namespace ELog.Application.Dispensing.LineClearances.Dto
{
    [AutoMapTo(typeof(LineClearanceTransaction))]
    public class CreateLineClearanceTransactionDto : EntityDto<int>
    {
        public DateTime LineClearanceDate { get; set; }
        public int CubicleId { get; set; }
        public int AreaId { get; set; }
        public int GroupId { get; set; }
        public int GroupName { get; set; }
        public int StatusId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? StopTime { get; set; }
        public int? ApprovedBy { get; set; }
        public int? VerifierBy { get; set; }
        public List<CheckpointDto> LineClearanceCheckpoints { get; set; }
        public bool IsVerified { get; set; }
        public bool CanApproved { get; set; }
    }
}