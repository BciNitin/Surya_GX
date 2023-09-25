using AutoMapper;

using ELog.Core.Entities;

namespace ELog.Application.Masters.WeighingMachines.Dto
{
    public class WeighingMachineMapProfile : Profile
    {
        public WeighingMachineMapProfile()
        {
            CreateMap<WeighingMachineDto, WeighingMachineMaster>();
            CreateMap<WeighingMachineDto, WeighingMachineMaster>()
                .ForMember(x => x.CreationTime, opt => opt.Ignore())
                .ForMember(x => x.Calibrations, opt => opt.Ignore())
                .ForMember(x => x.WeighingMachineTestConfigurations, opt => opt.Ignore());

            CreateMap<CreateWeighingMachineDto, WeighingMachineMaster>()
                 .ForMember(x => x.Calibrations, opt => opt.Ignore())
                 .ForMember(x => x.WeighingMachineTestConfigurations, opt => opt.Ignore());

            CreateMap<CalibrationFrequencyDto, CalibrationFrequencyMaster>();

            CreateMap<WeighingMachineTestconfigurationDto, WeighingMachineTestConfiguration>();
            CreateMap<WeighingMachineTestConfiguration, WeighingMachineTestconfigurationDto>();
        }
    }
}