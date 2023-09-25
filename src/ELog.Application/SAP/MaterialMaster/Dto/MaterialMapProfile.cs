using AutoMapper;

namespace ELog.Application.SAP.MaterialMaster.Dto
{
    public class MaterialMapProfile : Profile
    {
        public MaterialMapProfile()
        {
            CreateMap<SAPMaterial, Core.Entities.MaterialMaster>();
            CreateMap<Core.Entities.MaterialMaster, SAPMaterial>();
        }
    }
}