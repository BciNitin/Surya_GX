using Abp.Application.Services.Dto;
using Abp.AutoMapper;

using ELog.Application.Masters.InspectionChecklists.Dto;
using ELog.Core.Entities;

using System;
using System.Collections.Generic;

namespace ELog.Application.Dispensing.CubicleCleanings.Dto
{
    [AutoMapTo(typeof(CubicleCleaningTransaction))]
    public class CubicleCleaningTransactionDto : EntityDto<int>
    {
        public DateTime CleaningDate { get; set; }
        public int CubicleId { get; set; }
        public int TypeId { get; set; }
        public int StatusId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? StopTime { get; set; }
        public DateTime? VerifiedTime { get; set; }
        public int CleanerId { get; set; }
        public int? VerifierId { get; set; }
        public List<CheckpointDto> CubicleCleaningCheckpoints { get; set; }
        public bool IsUncleaned { get; set; }
        public bool IsVerified { get; set; }
        public bool CanApproved { get; set; }
        public string CleanerName { get; set; }
        public string CreatorName { get; set; }
        public string DoneBy { get; set; }
        public bool IsInValidTransaction { get; set; }
        public bool IsRejected { get; set; }
        public string Remark { get; set; }
    }
}