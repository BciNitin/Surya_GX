using Abp.AutoMapper;
using ELog.Core.Entities;

namespace ELog.Application.FormApprovalData.Dto
{
    [AutoMapTo(typeof(FormApproval))]
    public class CreateFormApprovalDataDto
    {
        //  public int Id { get; set; }
        public int FormId { get; set; }
        public int Status { get; set; }
        public string Remark { get; set; }
    }
}
