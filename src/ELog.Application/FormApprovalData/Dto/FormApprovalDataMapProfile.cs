using AutoMapper;
using ELog.Core.Entities;

namespace ELog.Application.FormApprovalData.Dto
{
    public class FormApprovalDataMapProfile : Profile
    {
        public FormApprovalDataMapProfile()
        {
            CreateMap<FormApprovalDataDto, FormApproval>();
            CreateMap<FormApprovalDataDto, FormApproval>();
            CreateMap<CreateFormApprovalDataDto, FormApproval>();

            CreateMap<FormApprovalDataDto, FormApproval>();
            CreateMap<FormApprovalDataDto, FormApprovalDataDto>();

        }
    }
}
