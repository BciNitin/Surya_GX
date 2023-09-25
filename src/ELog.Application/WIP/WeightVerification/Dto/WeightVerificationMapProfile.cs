using AutoMapper;

using ELog.Core.Entities;

namespace ELog.Application.WIP.WeightVerification.Dto
{
    public class WeightVerificationMapProfile : Profile
    {
        public WeightVerificationMapProfile()
        {
            CreateMap<WeightVerificationDto, WeightVerificationHeader>();

            CreateMap<WeightVerificationDto, WeightVerificationHeader>()
                .ForMember(x => x.CreationTime, opt => opt.Ignore());

            CreateMap<CreateWeightVerificationDto, WeightVerificationHeader>();
            CreateMap<WeightVerificationHeader, WeightVerificationDto>();

        }
    }
}
