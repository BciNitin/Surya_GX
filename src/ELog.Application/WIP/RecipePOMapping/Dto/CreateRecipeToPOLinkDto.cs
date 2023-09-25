using Abp.AutoMapper;
using ELog.Core.Entities;

namespace ELog.Application.WIP.RecipePOMapping.Dto
{
    [AutoMapTo(typeof(RecipeWiseProcessOrderMapping))]
    public class CreateRecipeToPOLinkDto
    {
        public int? ProcessOrderId { get; set; }
        public int? RecipeTransHdrId { get; set; }
        public string Remarks { get; set; }
        public int ProductId { get; set; }

    }
}
