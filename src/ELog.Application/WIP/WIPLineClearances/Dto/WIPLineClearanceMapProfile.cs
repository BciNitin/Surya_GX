using AutoMapper;

using ELog.Application.Masters.InspectionChecklists.Dto;
using ELog.Core.Entities;
namespace ELog.Application.WIP.WIPLineClearances.Dto
{
    public class WIPLineClearanceMapProfile : Profile
    {
        public WIPLineClearanceMapProfile()
        {
            CreateMap<WIPLineClearanceTransactionDto, WIPLineClearanceTransaction>()
                 .ForMember(x => x.CreationTime, opt => opt.Ignore());
            CreateMap<WIPLineClearanceTransaction, WIPLineClearanceTransactionDto>();
            CreateMap<CreateWIPLineClearanceDto, WIPLineClearanceTransactionDto>();
            CreateMap<WIPLineClearanceTransactionDto, CreateWIPLineClearanceDto>();
            CreateMap<CheckpointDto, WIPLineClearanceCheckpoints>()
            .ForMember(x => x.Remark, opt => opt.MapFrom(x => x.DiscrepancyRemark));
            CreateMap<WIPLineClearanceCheckpoints, CheckpointDto>()
            .ForMember(x => x.DiscrepancyRemark, opt => opt.MapFrom(x => x.Remark));
        }
    }
}
