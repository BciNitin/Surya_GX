using ELog.Core.Authorization;
using System.ComponentModel.DataAnnotations.Schema;

namespace ELog.Core.Entities
{
    [Table("CompRecipeTransDetlMapping")]
    public class CompRecipeTransDetlMapping : PMMSFullAudit
    {
        [ForeignKey("RecipeTransactionDetails")]
        public int RecipeTransactiondetailId { get; set; }
        public virtual RecipeTransactionDetails RecipeTransactionDetails { get; set; }
        public string Operation { get; set; }

        [ForeignKey("MaterialMaster")]
        public int ComponentId { get; set; }
        public virtual MaterialMaster MaterialMaster { get; set; }
        public bool IsActive { get; set; }
    }
}
