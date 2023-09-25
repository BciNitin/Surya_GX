using AutoMapper;

using ELog.Core.Entities;

namespace ELog.Application.Dispensing.PreStage.Dto
{
    public class PreStageMapProfile : Profile
    {
        public PreStageMapProfile()
        {
            CreateMap<PreStageDto, MaterialBatchDispensingHeader>()
                .ForMember(x => x.CreationTime, opt => opt.Ignore());
            CreateMap<MaterialBatchDispensingHeader, PreStageDto>()
                .ForMember(x => x.MaterialBatchDispensingHeaderId, opt => opt.Ignore());
        }
    }
}