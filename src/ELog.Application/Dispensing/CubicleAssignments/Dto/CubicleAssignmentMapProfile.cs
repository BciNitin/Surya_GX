using AutoMapper;

using ELog.Core.Entities;

namespace ELog.Application.Dispensing.CubicleAssignments.Dto
{
    public class CubicleAssignmentMapProfile : Profile
    {
        public CubicleAssignmentMapProfile()
        {
            CreateMap<CubicleAssignmentDto, CubicleAssignmentHeader>()
                                .ForMember(x => x.CreationTime, opt => opt.Ignore());

            CreateMap<CubicleAssignmentHeader, CubicleAssignmentDto>();
            CreateMap<CreateCubicleAssignmentDto, CubicleAssignmentHeader>();
            CreateMap<CubicleAssignmentDetailsDto, CubicleAssignmentDetail>()
                                .ForMember(x => x.CreationTime, opt => opt.Ignore());
            CreateMap<CubicleAssignmentDetail, CubicleAssignmentDetailsDto>();
        }
    }
}