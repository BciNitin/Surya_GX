using AutoMapper;

using ELog.Core.Entities;

namespace ELog.Application.Inward.WeightCaptures.Dto
{
    public class WeightCaptureMapProfile : Profile
    {
        public WeightCaptureMapProfile()
        {
            CreateMap<WeightCaptureHeader, WeightCaptureDto>()
                .ForMember(x => x.WeightCaptureDetailsDto, opt => opt.Ignore()).ReverseMap();
            CreateMap<WeightCaptureDetailsDto, WeightCaptureDetail>()
                .ForMember(x => x.CreationTime, opt => opt.Ignore());
        }
    }
}