using Abp.Application.Services.Dto;
using Abp.AutoMapper;

using ELog.Core.Entities;

using System;

namespace ELog.Application.Dispensing.CubicleCleanings.Dto
{
    [AutoMapTo(typeof(CubicleCleaningTransaction))]
    public class CreateCubicleCleaningTransactionDto : EntityDto<int>
    {
        public DateTime? CleaningDate { get; set; }
        public int CubicleId { get; set; }
        public int TypeId { get; set; }
        public int StatusId { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? StopTime { get; set; }
        public DateTime? VerifiedTime { get; set; }
        public int CleanerId { get; set; }
        public int? VerifierId { get; set; }
        public string Remark { get; set; }
        public string DoneBy { get; set; }
    }
}