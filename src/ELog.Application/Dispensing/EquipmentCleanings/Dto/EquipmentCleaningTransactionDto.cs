using Abp.Application.Services.Dto;
using Abp.AutoMapper;

using ELog.Application.Masters.InspectionChecklists.Dto;
using ELog.Core.Entities;

using System;
using System.Collections.Generic;

namespace ELog.Application.Dispensing.EquipmentCleanings.Dto
{
    [AutoMapTo(typeof(EquipmentCleaningTransaction))]
    public class EquipmentCleaningTransactionDto : EntityDto<int>
    {
        public DateTime CleaningDate { get; set; }
        public int EquipmentId { get; set; }
        public int CubicleId { get; set; }
        public int AreaId { get; set; }
        public int CleaningTypeId { get; set; }
        public int StatusId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? StopTime { get; set; }
        public DateTime? VerifiedTime { get; set; }
        public int CleanerId { get; set; }
        public int? VerifierId { get; set; }
        public string Remark { get; set; }
        public List<CheckpointDto> EquipmentCleaningCheckpoints { get; set; }
        public bool IsUncleaned { get; set; }
        public bool IsVerified { get; set; }
        public bool IsRejected { get; set; }
        public bool CanApproved { get; set; }
        public string CleanerName { get; set; }
        public string CreatorName { get; set; }
        public bool IsInValidTransaction { get; set; }

        public string DoneBy { get; set; }
    }
}