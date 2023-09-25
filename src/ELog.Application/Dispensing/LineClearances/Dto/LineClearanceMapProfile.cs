using AutoMapper;

using ELog.Application.Masters.InspectionChecklists.Dto;
using ELog.Core.Entities;

namespace ELog.Application.Dispensing.LineClearances.Dto
{
    public class LineClearanceMapProfile : Profile
    {
        public LineClearanceMapProfile()
        {
            CreateMap<LineClearanceTransactionDto, LineClearanceTransaction>()
                 .ForMember(x => x.CreationTime, opt => opt.Ignore());
            CreateMap<LineClearanceTransaction, LineClearanceTransactionDto>();
            CreateMap<CreateLineClearanceTransactionDto, LineClearanceTransactionDto>();
            CreateMap<LineClearanceTransactionDto, CreateLineClearanceTransactionDto>();
            CreateMap<CheckpointDto, LineClearanceCheckpoint>()
            .ForMember(x => x.Remark, opt => opt.MapFrom(x => x.DiscrepancyRemark));
            CreateMap<LineClearanceCheckpoint, CheckpointDto>()
            .ForMember(x => x.DiscrepancyRemark, opt => opt.MapFrom(x => x.Remark));
        }
    }
}