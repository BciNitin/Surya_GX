using AutoMapper;

using ELog.Core.Entities;
using ELog.Core.SAP;

namespace ELog.Application.Dispensing.IssueToProductions.Dto
{
    public class IssueToProductionMapProfile : Profile
    {
        public IssueToProductionMapProfile()
        {
            CreateMap<IssueToProductionDto, IssueToProduction>()
                                .ForMember(x => x.CreationTime, opt => opt.Ignore());

            CreateMap<IssueToProduction, IssueToProductionDto>();
            CreateMap<IssueToProductionRequestResponseDto, IssueToProductionDto>();

            CreateMap<IssueToProductionDto, IssueToProductionRequestResponseDto>();
            CreateMap<IssueToProductionRequestResponseDto, IssueToProduction>();

            CreateMap<IssueToProduction, IssueToProductionRequestResponseDto>();
            CreateMap<IssueToProductionRecord, IssueToProductionDto>();

            CreateMap<IssueToProductionDto, IssueToProductionRecord>();
            CreateMap<IssueToProductionRecord, IssueToProduction>();

            CreateMap<IssueToProduction, IssueToProductionRecord>();
        }
    }
}

