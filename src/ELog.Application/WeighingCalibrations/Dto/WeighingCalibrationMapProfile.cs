using AutoMapper;

using ELog.Core.Entities;

namespace ELog.Application.WeighingCalibrations.Dto
{
    public class WeighingCalibrationMapProfile : Profile
    {
        public WeighingCalibrationMapProfile()
        {
            CreateMap<CreateWeighingCalibrationDto, WMCalibrationHeader>();

            CreateMap<WeighingCalibrationDto, WMCalibrationHeader>();

            CreateMap<WMCalibrationHeader, WeighingCalibrationDto>();

            CreateMap<WeighingCalibrationDetailDto, WMCalibrationDetail>()
                .ForMember(x => x.Id, opt => opt.Ignore());

            CreateMap<WMCalibrationDetail, WeighingCalibrationDetailDto>();

            CreateMap<WeighingCalibrationEccentricityTestDto, WMCalibrationEccentricityTest>()
                .ForMember(x => x.Id, opt => opt.Ignore());

            CreateMap<WMCalibrationEccentricityTest, WeighingCalibrationEccentricityTestDto>();

            CreateMap<WeighingCalibrationLinearityTestDto, WMCalibrationLinearityTest>()
                .ForMember(x => x.Id, opt => opt.Ignore());

            CreateMap<WMCalibrationLinearityTest, WeighingCalibrationLinearityTestDto>();

            CreateMap<WeighingCalibrationRepeatabilityTestDto, WMCalibrationRepeatabilityTest>()
              .ForMember(x => x.Id, opt => opt.Ignore());

            CreateMap<WMCalibrationRepeatabilityTest, WeighingCalibrationRepeatabilityTestDto>();

            CreateMap<CalibrationDto, WeighingCalibrationDetailDto>();

            CreateMap<WeighingCalibrationCommonTestDto, WeighingCalibrationEccentricityTestDto>()
                .ForMember(x => x.CalculatedCapacityWeight, opt => opt.Ignore());
            CreateMap<WeighingCalibrationCommonTestDto, WeighingCalibrationLinearityTestDto>();
            CreateMap<WeighingCalibrationCommonTestDto, WeighingCalibrationRepeatabilityTestDto>()
                .ForMember(x => x.CalculatedCapacityWeight, opt => opt.Ignore());
        }
    }
}