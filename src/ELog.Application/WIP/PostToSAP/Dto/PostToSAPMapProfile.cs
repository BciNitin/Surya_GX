using AutoMapper;
using ELog.Core.Entities;

namespace ELog.Application.WIP.PostToSAP.Dto
{
    public class PostToSAPMapProfile : Profile
    {
        public PostToSAPMapProfile()
        {
            CreateMap<PostToSAPDto, PostWIPDataToSAP>();
            CreateMap<PostToSAPDto, PostWIPDataToSAP>()
                .ForMember(x => x.CreationTime, opt => opt.Ignore());

            CreateMap<CreatePostToSAPDto, PostWIPDataToSAP>();
            CreateMap<PostToSAPListDto, PostWIPDataToSAP>();
            CreateMap<PostWIPDataToSAP, PostToSAPListDto>();

        }
    }
}
