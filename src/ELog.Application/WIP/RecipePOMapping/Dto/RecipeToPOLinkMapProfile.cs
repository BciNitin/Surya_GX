using AutoMapper;
using ELog.Core.Entities;

namespace ELog.Application.WIP.RecipePOMapping.Dto
{
    public class RecipeToPOLinkMapProfile : Profile
    {
        public RecipeToPOLinkMapProfile()
        {
            CreateMap<RecipeToPOLinkDto, RecipeWiseProcessOrderMapping>()
            .ForMember(x => x.IsActive, opt => opt.Ignore());
            CreateMap<RecipeWiseProcessOrderMapping, RecipeToPOLinkDto>();
            CreateMap<RecipeToPOLinkDto, RecipeWiseProcessOrderMapping>()
                .ForMember(x => x.CreationTime, opt => opt.Ignore());

            CreateMap<CreateRecipeToPOLinkDto, RecipeToPOLinkDto>();
            CreateMap<RecipePOMappingListDto, RecipeToPOLinkDto>();
            CreateMap<RecipeToPOLinkDto, RecipePOMappingListDto>();
            CreateMap<RecipeWiseProcessOrderMapping, RecipePOMappingListDto>();
            CreateMap<RecipePOMappingListDto, RecipeWiseProcessOrderMapping>();
            CreateMap<CreateRecipeMaster1Dto, RecipeTransactionHeader>()
    .ForMember(x => x.CreationTime, opt => opt.Ignore());

            CreateMap<RecipeTransactionHeader, CreateRecipeMaster1Dto>();
            CreateMap<CreayeRecipeMasterdetail1, RecipeTransactionDetails>()
               .ForMember(x => x.RecipeTransactionHeaderId, opt => opt.MapFrom(x => x.RecipeTransHdrId))
                .ForMember(x => x.CreationTime, opt => opt.Ignore());

            CreateMap<RecipeTransactionDetails, CreayeRecipeMasterdetail1>()
                .ForMember(x => x.RecipeTransHdrId, opt => opt.MapFrom(x => x.RecipeTransactionHeaderId));

        }
    }
}
