using AutoMapper;
using ELog.Core.Entities;

namespace ELog.Application.TransactionApprovalMatrix.ApprovalLevel.Dto
{
    class ApprovalLevelMapProfile : Profile
    {
        public ApprovalLevelMapProfile()
        {
            CreateMap<ApprovalLevelDto, ApprovalLevelMaster>();
            CreateMap<ApprovalLevelDto, ApprovalLevelMaster>()
                .ForMember(x => x.CreationTime, opt => opt.Ignore());

            CreateMap<CreateApprovalLevelDto, ApprovalLevelMaster>();
            CreateMap<ApprovalLevelListDto, ApprovalLevelMaster>();
            CreateMap<ApprovalLevelMaster, ApprovalLevelDto>();

        }
    }
}