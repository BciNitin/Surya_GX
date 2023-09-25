using AutoMapper;

using ELog.Core.Entities;

using System;

namespace ELog.Application.Inward.PutAways.Dto
{
    public class PutAwayBinToBinTransferMapProfile : Profile
    {
        public PutAwayBinToBinTransferMapProfile()
        {
            CreateMap<PutAwayBinToBinTransferDto, PutAwayBinToBinTransfer>()
                            .ForMember(x => x.CreationTime, opt => opt.Ignore());

            CreateMap<PutAwayBinToBinTransfer, PutAwayBinToBinTransferDto>();
            CreateMap<PutAwayBinToBinTransfer, CreatePutAwayBinToBinTransferDto>();
            CreateMap<CreatePutAwayBinToBinTransferDto, CreatePutAwayBinToBinTransferDto>();
            //for guid to string or string to guid
            CreateMap<string, Guid>().ConvertUsing(s => Guid.Parse(s));
            CreateMap<string, Guid?>().ConvertUsing(s => String.IsNullOrWhiteSpace(s) ? (Guid?)null : Guid.Parse(s));
            CreateMap<Guid, string>().ConvertUsing(g => g.ToString("N"));
        }
    }
}
