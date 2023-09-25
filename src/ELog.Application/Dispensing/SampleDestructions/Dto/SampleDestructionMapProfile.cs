using AutoMapper;

namespace ELog.Application.Dispensing.SampleDestructions.Dto
{
    public class SampleDestructionMapProfile : Profile
    {
        public SampleDestructionMapProfile()
        {
            CreateMap<SampleDestructionDto, ELog.Core.Entities.SampleDestruction>()
                   .ForMember(x => x.CreationTime, opt => opt.Ignore())
                     .ForMember(dest => dest.ContainerMaterialBarcode, opt => opt.MapFrom(src => src.MaterialContainerBarCode))
                     .ForMember(dest => dest.WeighingMachineId, opt => opt.MapFrom(src => src.BalanceId))
                     .ForMember(dest => dest.SAPBatchNumber, opt => opt.MapFrom(src => src.SAPBatchNo));
        }
    }
}