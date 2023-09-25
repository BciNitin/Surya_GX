using AutoMapper;
using ELog.Core.Entities;

namespace ELog.Application.FinishedGoods.SAP_FG.Dto
{
    public class OBDDetailProfile : Profile
    {
        public OBDDetailProfile()
        {
            CreateMap<OBDDetailDto, OBDDetails>();
            CreateMap<OBDDetails, OBDDetailDto>();

        }
    }
}
