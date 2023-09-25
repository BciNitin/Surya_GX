using AutoMapper;

using ELog.Core.Entities;

namespace ELog.Application.Masters.Departments.Dto
{
    public class DepartmentMapProfile : Profile
    {
        public DepartmentMapProfile()
        {
            CreateMap<DepartmentDto, DepartmentMaster>();
            CreateMap<DepartmentDto, DepartmentMaster>()
                .ForMember(x => x.CreationTime, opt => opt.Ignore());

            CreateMap<CreateDepartmentDto, DepartmentMaster>();
        }
    }
}