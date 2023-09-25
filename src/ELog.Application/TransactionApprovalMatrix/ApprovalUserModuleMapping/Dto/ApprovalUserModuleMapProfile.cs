using AutoMapper;
using ELog.Core.Entities;


namespace ELog.Application.TransactionApprovalMatrix.ApprovalUserModuleMapping.Dto
{
    public class ApprovalUserModuleMapProfile : Profile
    {
        public ApprovalUserModuleMapProfile()
        {
            CreateMap<ApprovalUserModuleMappingDto, ApprovalUserModuleMappingMaster>();
            CreateMap<ApprovalUserModuleMappingDto, ApprovalUserModuleMappingMaster>()
                .ForMember(x => x.CreationTime, opt => opt.Ignore());

            CreateMap<CreateApprovalUserModuleMappingDto, ApprovalUserModuleMappingMaster>();
            CreateMap<ApprovalUserModuleMappingMaster, ApprovalUserModuleMappingDto>();
        }
    }
}
