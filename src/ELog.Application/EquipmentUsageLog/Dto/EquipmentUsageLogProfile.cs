using AutoMapper;
using ELog.Application.Masters.InspectionChecklists.Dto;
using ELog.Core.Entities;

namespace ELog.Application.EquipmentUsageLog.Dto
{
    public class EquipmentUsageLogProfile : Profile
    {
        public EquipmentUsageLogProfile()
        {
            CreateMap<EquipmentUsageLogDto, ELog.Core.Entities.EquipmentUsageLog>();
            CreateMap<EquipmentUsageLogDto, ELog.Core.Entities.EquipmentUsageLog>()
                .ForMember(x => x.CreationTime, opt => opt.Ignore());
            CreateMap<CreateEquipmentUsageLogDto, ELog.Core.Entities.EquipmentUsageLog>();
            CreateMap<ELog.Core.Entities.EquipmentUsageLog, EquipmentUsageLogDto>();

            //CreateMap<UpdateEquipmentUsageLogDto, ELog.Core.Entities.EquipmentUsageLog>();

            CreateMap<CheckpointDto, EquipmentUsageLogList>()
                .ForMember(x => x.Id, opt => opt.Ignore());
            CreateMap<EquipmentUsageLogList, CheckpointDto>();

        }
    }
}
