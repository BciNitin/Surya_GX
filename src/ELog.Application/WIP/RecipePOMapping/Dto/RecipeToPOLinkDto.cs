using Abp.Application.Services.Dto;

namespace ELog.Application.WIP.RecipePOMapping.Dto
{

    public class RecipeToPOLinkDto : EntityDto<int>
    {
        public int? ProcessOrderId { get; set; }
        public int? RecipeTransHdrId { get; set; }
        public string Remarks { get; set; }
        public int ProductId { get; set; }

    }
}
