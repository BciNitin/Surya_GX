using AutoMapper;

using ELog.Application.Masters.InspectionChecklists.Dto;
using ELog.Core.Entities;

namespace ELog.Application.Inward.VehicleInspections.Dto
{
    public class VehicleInspectionMapProfile : Profile
    {
        public VehicleInspectionMapProfile()
        {
            CreateMap<VehicleInspectionDto, VehicleInspectionHeader>();
            CreateMap<VehicleInspectionDto, VehicleInspectionHeader>()
                .ForMember(x => x.CreationTime, opt => opt.Ignore());
            CreateMap<CreateVehicleInspectionDto, VehicleInspectionHeader>();
            CreateMap<UpdateVehicleInspectionDto, VehicleInspectionHeader>();

            CreateMap<CheckpointDto, VehicleInspectionDetail>()
                .ForMember(x => x.Id, opt => opt.Ignore());
            CreateMap<VehicleInspectionDetail, CheckpointDto>();
            CreateMap<VehicleInspectionDetailsDto, VehicleInspectionDetail>()
                .ForMember(x => x.CreationTime, opt => opt.Ignore());
            CreateMap<UpdateVehicleInspectionDetailDto, VehicleInspectionDetail>().ReverseMap();
        }
    }
}