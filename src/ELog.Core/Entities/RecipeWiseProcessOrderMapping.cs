using ELog.Core.Authorization;
using System.ComponentModel.DataAnnotations.Schema;

namespace ELog.Core.Entities
{
    [Table("RecipeWiseProcessOrderMapping")]
    public class RecipeWiseProcessOrderMapping : PMMSFullAudit
    {
        public int ProcessOrderId { get; set; }

        [ForeignKey("RecipeTransactionHeader")]
        public int RecipeTransHdrId { get; set; }
        public string Remarks { get; set; }
        public int IsActive { get; set; }

        public int ProductId { get; set; }


    }
}
