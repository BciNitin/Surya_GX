using AutoMapper;
using ELog.Core.Entities;

namespace ELog.Application.Recipe.Dto
{
    public class RecipeMapProfile : Profile
    {
        public RecipeMapProfile()
        {
            CreateMap<CreateRecipeMasterDto, RecipeTransactionHeader>()
                .ForMember(x => x.CreationTime, opt => opt.Ignore());


            CreateMap<RecipeTransactionHeader, CreateRecipeMasterDto>();


            CreateMap<CreayeRecipeMasterdetail, RecipeTransactionDetails>()
                .ForMember(x => x.RecipeTransactionHeaderId, opt => opt.MapFrom(x => x.RecipeTransHdrId))
                .ForMember(x => x.CubicalRecipeTranDetlMapping, opt => opt.MapFrom(x => x.CreateCubicalRecipeTranDetlMapping))
                .ForMember(x => x.CompRecipeTransDetlMapping, opt => opt.MapFrom(x => x.CreateCompRecipeTransDetlMapping))
                .ForMember(x => x.CreationTime, opt => opt.Ignore());

            CreateMap<RecipeTransactionDetails, CreayeRecipeMasterdetail>()
                .ForMember(x => x.RecipeTransHdrId, opt => opt.MapFrom(x => x.RecipeTransactionHeaderId))
                .ForMember(x => x.CreateCubicalRecipeTranDetlMapping, opt => opt.MapFrom(x => x.CubicalRecipeTranDetlMapping))
                .ForMember(x => x.CreateCompRecipeTransDetlMapping, opt => opt.MapFrom(x => x.CompRecipeTransDetlMapping));

            CreateMap<CreateCubicalRecipeTranDetlMapping, CubicalRecipeTranDetlMapping>()
                .ForMember(x => x.CreationTime, opt => opt.Ignore())
                .ForMember(x => x.CreatorUserId, opt => opt.Ignore());

            CreateMap<CubicalRecipeTranDetlMapping, CreateCubicalRecipeTranDetlMapping>();

            CreateMap<CreateCompRecipeTransDetlMapping, CompRecipeTransDetlMapping>()
                .ForMember(x => x.CreationTime, opt => opt.Ignore())
                .ForMember(x => x.CreatorUserId, opt => opt.Ignore());

            CreateMap<CompRecipeTransDetlMapping, CreateCompRecipeTransDetlMapping>();

            CreateMap<RecipeMasterDto, RecipeTransactionHeader>();
            CreateMap<RecipeMasterDto, RecipeTransactionHeader>()
                .ForMember(x => x.CreationTime, opt => opt.Ignore());








        }
    }
}
