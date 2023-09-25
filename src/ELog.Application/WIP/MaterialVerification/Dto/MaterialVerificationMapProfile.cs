using AutoMapper;
using ELog.Core.Entities;


namespace ELog.Application.WIP.MaterialVerification.Dto
{
    public class MaterialVerificationMapProfile : Profile
    {
        public MaterialVerificationMapProfile()
        {
            CreateMap<MaterialVerificationDto, WIPMaterialVerification>()
                                .ForMember(x => x.CreationTime, opt => opt.Ignore());

            CreateMap<WIPMaterialVerification, MaterialVerificationDto>();
            CreateMap<CreateMaterialVerificationDto, WIPMaterialVerification>();
            CreateMap<CreateMaterialVerificationDto, CreateMaterialVerificationDto>();
            CreateMap<MaterialVerificationListDto, WIPMaterialVerification>()
                                .ForMember(x => x.CreationTime, opt => opt.Ignore());
            //.ForMember(x => x.Id, opt => opt.Ignore());
            CreateMap<WIPMaterialVerification, MaterialVerificationListDto>();
        }
    }
}
