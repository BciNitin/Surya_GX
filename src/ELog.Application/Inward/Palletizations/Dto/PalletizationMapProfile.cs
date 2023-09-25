using AutoMapper;

using ELog.Core.Entities;

using System;

namespace ELog.Application.Inward.Palletizations.Dto
{
    public class PalletizationMapProfile : Profile
    {
        public PalletizationMapProfile()
        {
            CreateMap<PalletizationDto, Palletization>()
                .ForMember(x => x.TransactionId, opt => opt.MapFrom(src => src.TransactionId))
                .ForMember(x => x.CreationTime, opt => opt.Ignore());
            CreateMap<CreatePalletizationDto, Palletization>().ForMember(x => x.TransactionId, opt => opt.MapFrom(src => src.TransactionId));
            CreateMap<Palletization, PalletizationDto>()
               .ForMember(dest => dest.TransactionId, opt => opt.MapFrom(src => src.TransactionId));
            CreateMap<Palletization, CreatePalletizationDto>()
              .ForMember(dest => dest.TransactionId, opt => opt.MapFrom(src => src.TransactionId));

            //for guid to string or string to guid
            CreateMap<string, Guid>().ConvertUsing(s => Guid.Parse(s));
            CreateMap<string, Guid?>().ConvertUsing(s => String.IsNullOrWhiteSpace(s) ? (Guid?)null : Guid.Parse(s));
            CreateMap<Guid, string>().ConvertUsing(g => g.ToString("N"));
        }
    }
}