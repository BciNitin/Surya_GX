using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using ELog.Core.Entities;

namespace ELog.Application.WIP.RecipePOMapping.Dto
{
    [AutoMapFrom(typeof(RecipeTransactionDetails))]
    public class RecipeListDto : EntityDto<int>
    {
        public int RecipeTransHdrId { get; set; }
        public string Operation { get; set; }
        public string Stage { get; set; }
        public string NextOperation { get; set; }
        public string Component { get; set; }
        public bool IsWeightRequired { get; set; }
        public bool IsLebalPrintingRequired { get; set; }
        public bool IsVerificationReq { get; set; }
        public bool InProcessSamplingRequired { get; set; }
        public bool IsSamplingReq { get; set; }
        public bool IsActive { get; set; }
    }
}
