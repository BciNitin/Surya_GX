using Abp.Application.Services.Dto;
using Abp.AutoMapper;

using ELog.Core.MultiTenancy;
using System.Collections.Generic;

namespace ELog.Application.Sessions.Dto
{
    [AutoMapFrom(typeof(Tenant))]
    public class Role : EntityDto
    {
        public string Name { get; set; }
    }
}
