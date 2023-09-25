using Abp.Application.Services.Dto;

namespace ELog.Application.WIP.RecipePOMapping.Dto
{

    public class RecipeToPODto : EntityDto<int>
    {
        public int? ProcessOrderId { get; set; }
        public int? RecipeTransHdrId { get; set; }
        public string ProcessOrderNo { get; set; }
        public string RecipeNo { get; set; }

        public string DocumentVersion { get; set; }

    }
}
