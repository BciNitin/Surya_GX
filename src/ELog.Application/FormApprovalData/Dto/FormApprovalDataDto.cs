using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using ELog.Core.Entities;

namespace ELog.Application.FormApprovalData.Dto
{
    [AutoMapFrom(typeof(FormApproval))]
    public class FormApprovalDataDto : EntityDto<int>
    {
        //      public int Id { get; set; }
        public int FormId { get; set; }
        public int Status { get; set; }
        public string Remark { get; set; }
    }
}
