using AutoMapper;
using ELog.Core.Entities;

namespace ELog.Application.WIP.CubicleAssignments.Dto
{
    public class CubicleAssignmentsMapProfile : Profile
    {
        public CubicleAssignmentsMapProfile()
        {
            CreateMap<CubicleAssignmentsDto, CubicleAssignmentWIP>()
                                .ForMember(x => x.CreationTime, opt => opt.Ignore());

            CreateMap<CubicleAssignmentWIP, CubicleAssignmentsDto>();
            CreateMap<CreateCubicleAssignmentsDto, CubicleAssignmentWIP>();
            CreateMap<CreateCubicleAssignmentsDto, CreateCubicleAssignmentsDto>();
            CreateMap<CubicleAssignmentsListDto, CubicleAssignmentWIP>()
                                .ForMember(x => x.CreationTime, opt => opt.Ignore());
            //.ForMember(x => x.Id, opt => opt.Ignore());
            CreateMap<CubicleAssignmentWIP, CubicleAssignmentsListDto>();
        }
    }
}
