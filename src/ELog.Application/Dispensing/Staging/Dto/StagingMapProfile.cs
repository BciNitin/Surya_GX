using AutoMapper;

using ELog.Core.Entities;

namespace ELog.Application.Dispensing.Stage.Dto
{
    public class StagingMapProfile : Profile
    {
        public StagingMapProfile()
        {
            CreateMap<StagingDto, MaterialBatchDispensingHeader>()
                .ForMember(x => x.CreationTime, opt => opt.Ignore());
            CreateMap<MaterialBatchDispensingHeader, StagingDto>()
                .ForMember(x => x.MaterialBatchDispensingHeaderId, opt => opt.Ignore());
        }
    }
}