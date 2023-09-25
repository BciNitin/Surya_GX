using AutoMapper;
using ELog.Core.Entities;

namespace ELog.Application.WIP.Consumption.Dto
{
    public class ConsumptionMapProfile : Profile
    {
        public ConsumptionMapProfile()
        {
            CreateMap<ConsumptionDto, ELog.Core.Entities.Consumption>()
                                .ForMember(x => x.CreationTime, opt => opt.Ignore());

            CreateMap<ELog.Core.Entities.Consumption, ConsumptionDto>();
            CreateMap<CreateConsumptionHeaderDto, ELog.Core.Entities.Consumption>();
            CreateMap<CreateConsumptionHeaderDto, CreateConsumptionHeaderDto>();
            CreateMap<ConsumptionDetailDto, ConsumptionDetails>()
                                .ForMember(x => x.CreationTime, opt => opt.Ignore());
            //.ForMember(x => x.Id, opt => opt.Ignore());
            CreateMap<ConsumptionDetails, ConsumptionDetailDto>();
        }
    }
}
