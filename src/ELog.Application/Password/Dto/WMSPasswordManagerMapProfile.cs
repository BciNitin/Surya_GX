using AutoMapper;

using ELog.Core.Entities;

using System;

namespace ELog.Application.Password.Dto
{
    public class WMSPasswordManagerMapProfile : Profile
    {
        public WMSPasswordManagerMapProfile()
        {

            CreateMap<WMSPasswordManagerDto, WMSPasswordManager>()
                            .ForMember(x => x.CreationTime, opt => opt.Ignore());
            CreateMap<WMSPasswordManager, WMSPasswordManagerDto>();

            //for guid to string or string to guid
            CreateMap<string, Guid>().ConvertUsing(s => Guid.Parse(s));
            CreateMap<string, Guid?>().ConvertUsing(s => String.IsNullOrWhiteSpace(s) ? (Guid?)null : Guid.Parse(s));
            CreateMap<Guid, string>().ConvertUsing(g => g.ToString("N"));
        }

    }
}



