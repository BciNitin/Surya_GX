using ELog.Core.Authorization;
using System.ComponentModel.DataAnnotations.Schema;

namespace ELog.Core.Entities
{
    [Table("CubicalRecipeTranDetlMapping")]
    public class CubicalRecipeTranDetlMapping : PMMSFullAudit
    {
        [ForeignKey("RecipeTransactionDetails")]
        public int RecipeTransactiondetailId { get; set; }
        public virtual RecipeTransactionDetails RecipeTransactionDetails { get; set; }
        public string Operation { get; set; }

        [ForeignKey("CubicleMaster")]
        public int CubicalId { get; set; }
        public virtual CubicleMaster CubicleMaster { get; set; }
        public bool IsActive { get; set; }
    }
}
