using AutoMapper;
using Elog.Core.Entities;

namespace Elog.Application.ClientForms.Dto
{
    public class ClientFormsMapProfile : Profile
    {
        public ClientFormsMapProfile()
        {
            CreateMap<ClientFormsDto, ClientForm>();
            CreateMap<ClientFormsDto, ClientForm>();




            CreateMap<CreateClientFormsDto, ClientForm>();


            CreateMap<ClientFormsListDto, ClientForm>();
            CreateMap<ClientForm, ClientFormsListDto>();
        }

    }
}
