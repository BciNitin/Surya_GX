using AutoMapper;
using Elog.Core.Entities;

namespace Elog.Application.ElogControls.Dto
{
    public class ElogControlsMapProfile : Profile
    {
        public ElogControlsMapProfile()
        {
            CreateMap<ElogControlsDto, ElogControl>();
            //CreateMap<ElogControlsDto, ElogControl>()
            //    .ForMember(x => x.CreationDate, opt => opt.Ignore());




            CreateMap<CreateElogControlsDto, ElogControl>();


            //CreateMap<ClientFormsListDto, ClientForm>();
            //CreateMap<ClientForm, ClientFormsListDto>();
        }

    }
}
