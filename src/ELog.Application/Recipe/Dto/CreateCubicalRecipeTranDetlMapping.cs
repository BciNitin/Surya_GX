using Abp.AutoMapper;
using ELog.Core.Entities;

namespace ELog.Application.Recipe.Dto
{
    [AutoMapTo(typeof(CubicalRecipeTranDetlMapping))]
    public class CreateCubicalRecipeTranDetlMapping
    {
        public int? RecipeTransactiondetailId { get; set; }
        public string Operation { get; set; }
        public int CubicalId { get; set; }
        public bool IsActive { get; set; }
    }
}
