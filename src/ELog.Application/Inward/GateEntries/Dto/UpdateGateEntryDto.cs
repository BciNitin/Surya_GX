using Abp.Application.Services.Dto;
using Abp.AutoMapper;

using ELog.Core.Entities;

namespace ELog.Application.Inward.GateEntries.Dto
{
    [AutoMapFrom(typeof(GateEntry))]
    public class UpdateGateEntryDto : EntityDto<int>
    {
        public int PrintCount { get; set; }
        public bool IsActive { get; set; }
        public int? PrinterId { get; set; }
    }
}