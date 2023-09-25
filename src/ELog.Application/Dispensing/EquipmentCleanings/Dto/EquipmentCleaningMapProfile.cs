using AutoMapper;

using ELog.Application.Masters.InspectionChecklists.Dto;
using ELog.Core.Entities;

namespace ELog.Application.Dispensing.EquipmentCleanings.Dto
{
    public class EquipmentCleaningMapProfile : Profile
    {
        public EquipmentCleaningMapProfile()
        {
            CreateMap<EquipmentCleaningTransactionDto, EquipmentCleaningTransaction>()
                 .ForMember(x => x.CreationTime, opt => opt.Ignore());
            CreateMap<EquipmentCleaningStatusDto, EquipmentCleaningStatus>()
                .ForMember(x => x.CreationTime, opt => opt.Ignore());
            CreateMap<EquipmentCleaningTransaction, EquipmentCleaningTransactionDto>();
            CreateMap<EquipmentCleaningStatus, EquipmentCleaningStatusDto>();
            CreateMap<CheckpointDto, EquipmentCleaningCheckpoint>()
            .ForMember(x => x.Remark, opt => opt.MapFrom(x => x.DiscrepancyRemark));
            CreateMap<EquipmentCleaningCheckpoint, CheckpointDto>()
            .ForMember(x => x.DiscrepancyRemark, opt => opt.MapFrom(x => x.Remark));
        }
    }
}