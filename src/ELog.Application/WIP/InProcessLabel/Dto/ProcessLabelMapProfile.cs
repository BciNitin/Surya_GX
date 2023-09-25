using AutoMapper;
using ELog.Core.Entities;

namespace ELog.Application.WIP.InProcessLabel.Dto
{
    public class ProcessLabelMapProfile : Profile
    {
        public ProcessLabelMapProfile()
        {
            CreateMap<ProcessLabelDto, InProcessLabelDetails>();
            CreateMap<ProcessLabelDto, InProcessLabelDetails>()
                .ForMember(x => x.CreationTime, opt => opt.Ignore());

            CreateMap<CreateProcessLabelDto, InProcessLabelDetails>();
            CreateMap<ProcessLabelListDto, InProcessLabelDetails>();
            CreateMap<InProcessLabelDetails, ProcessLabelListDto>();

        }
    }
}
