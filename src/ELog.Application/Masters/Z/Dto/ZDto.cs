using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using ELog.Core.Entities;

namespace ELog.Application.Masters.Z.Dto
{
    [AutoMapFrom(typeof(ZMaster))]
    public class ZDto : EntityDto<int>
    {
        public string ZField { get; set; }
        public string DescriptionField { get; set; }
    }
}