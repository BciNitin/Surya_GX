using AutoMapper;

using ELog.Application.Masters.InspectionChecklists.Dto;
using ELog.Core.Entities;

namespace ELog.Application.Dispensing.CubicleCleanings.Dto
{
    public class CubicleCleaningMapProfile : Profile
    {
        public CubicleCleaningMapProfile()
        {
            CreateMap<CubicleCleaningTransactionDto, CubicleCleaningTransaction>()
                 .ForMember(x => x.CreationTime, opt => opt.Ignore());
            CreateMap<CubicleCleaningDailyStatusDto, CubicleCleaningDailyStatus>()
                .ForMember(x => x.CreationTime, opt => opt.Ignore());
            CreateMap<CubicleCleaningTransaction, CubicleCleaningTransactionDto>();
            CreateMap<CubicleCleaningDailyStatus, CubicleCleaningDailyStatusDto>();
            CreateMap<CheckpointDto, CubicleCleaningCheckpoint>()
            .ForMember(x => x.Remark, opt => opt.MapFrom(x => x.DiscrepancyRemark));
            CreateMap<CubicleCleaningCheckpoint, CheckpointDto>()
            .ForMember(x => x.DiscrepancyRemark, opt => opt.MapFrom(x => x.Remark));
        }
    }
}