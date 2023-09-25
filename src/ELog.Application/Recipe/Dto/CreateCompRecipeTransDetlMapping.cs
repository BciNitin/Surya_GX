using Abp.AutoMapper;
using ELog.Core.Entities;

namespace ELog.Application.Recipe.Dto
{
    [AutoMapTo(typeof(CompRecipeTransDetlMapping))]
    public class CreateCompRecipeTransDetlMapping
    {
        public int RecipeTransactiondetailId { get; set; }
        public string Operation { get; set; }
        public int? ComponentId { get; set; }
        public string ComponentCode { get; set; }
        public bool IsActive { get; set; }
    }
}
