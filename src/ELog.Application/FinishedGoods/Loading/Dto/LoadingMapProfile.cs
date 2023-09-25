using AutoMapper;
using ELog.Core.Entities;
using System;

namespace ELog.Application.FinishedGoods.Picking.Dto
{
    public class LoadingMapProfile : Profile
    {
        public LoadingMapProfile()
        {

            CreateMap<LoadingDto, Loading>()
                            .ForMember(x => x.CreationTime, opt => opt.Ignore());
            CreateMap<Loading, LoadingDto>();

            //for guid to string or string to guid
            CreateMap<string, Guid>().ConvertUsing(s => Guid.Parse(s));
            CreateMap<string, Guid?>().ConvertUsing(s => String.IsNullOrWhiteSpace(s) ? (Guid?)null : Guid.Parse(s));
            CreateMap<Guid, string>().ConvertUsing(g => g.ToString("N"));
        }
    }
}
