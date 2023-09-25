using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using ELog.Core.Entities;

namespace ELog.Application.Recipe.Dto
{
    [AutoMapFrom(typeof(RecipeMaster))]
    public class RecipeMasterListDto : EntityDto<int>
    {
        public int RecipeId { get; set; }
        public int ProductId { get; set; }
        public string RecipeNo { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string ProductNo { get; set; }
        public string DocVersion { get; set; }
        public int? TenantId { get; set; }

        public string UserEnteredProductCode { get; set; }
        public string UserEnteredProductName { get; set; }
        public int? ApprovedLevelId { get; set; }
        public string ApprovalStatus { get; set; }


    }
}
