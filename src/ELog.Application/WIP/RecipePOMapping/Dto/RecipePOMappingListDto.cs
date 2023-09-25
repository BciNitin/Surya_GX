using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using ELog.Core.Entities;

namespace ELog.Application.WIP.RecipePOMapping.Dto
{
    [AutoMapFrom(typeof(RecipeTransactionHeader))]
    public class RecipePOMappingListDto : EntityDto<int>
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
    }
}
