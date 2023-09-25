using ELog.Core.Authorization;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ELog.Core.Entities
{
    [Table("RecipeTransactionDetails")]
    public class RecipeTransactionDetails : PMMSFullAudit
    {
        [ForeignKey("RecipeTransactionHeaderId")]
        public int RecipeTransactionHeaderId { get; set; }
        public virtual RecipeTransactionHeader RecipeTransactionHeader { get; set; }
        public string Operation { get; set; }
        public string Stage { get; set; }
        public string NextOperation { get; set; }
        public string Component { get; set; }
        public bool IsWeightRequired { get; set; }
        public bool IsLebalPrintingRequired { get; set; }
        public bool IsVerificationReq { get; set; }
        public bool InProcessSamplingRequired { get; set; }
        public bool IsSamplingReq { get; set; }
        public bool IsActive { get; set; }
        public string DocumentVersion { get; set; }
        public string RecipeNo { get; set; }
        public string MaterialDescription { get; set; }


        public virtual ICollection<CubicalRecipeTranDetlMapping> CubicalRecipeTranDetlMapping { get; set; }
        public virtual ICollection<CompRecipeTransDetlMapping> CompRecipeTransDetlMapping { get; set; }
    }
}
