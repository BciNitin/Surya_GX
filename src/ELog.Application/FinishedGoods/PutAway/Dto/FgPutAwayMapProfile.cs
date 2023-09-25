using AutoMapper;

using ELog.Core.Entities;

using System;

namespace ELog.Application.FinishedGoods.PutAway.Dto
{

    public class FgPutAwayMapProfile : Profile
    {

        public FgPutAwayMapProfile()
        {

            CreateMap<FgPutAwayDto, FgPutAway>()
                            .ForMember(x => x.CreationTime, opt => opt.Ignore());
            CreateMap<FgPutAway, FgPutAwayDto>();

            //for guid to string or string to guid
            CreateMap<string, Guid>().ConvertUsing(s => Guid.Parse(s));
            CreateMap<string, Guid?>().ConvertUsing(s => String.IsNullOrWhiteSpace(s) ? (Guid?)null : Guid.Parse(s));
            CreateMap<Guid, string>().ConvertUsing(g => g.ToString("N"));
        }

    }

}
