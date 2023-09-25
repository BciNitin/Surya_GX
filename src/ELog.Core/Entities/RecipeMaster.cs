using ELog.Core.Authorization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ELog.Core.Entities
{
    [Table("RecipeMaster")]
    public class RecipeMaster : PMMSFullAudit
    {
        public int RecipeId { get; set; }

        [StringLength(PMMSConsts.MaxAreaNameLength)]
        public string ProductCode { get; set; }

        [StringLength(PMMSConsts.MaxAreaNameLength)]
        public string ProductName { get; set; }

        [StringLength(PMMSConsts.MaxAreaNameLength)]
        public string ProductNo { get; set; }

        [StringLength(PMMSConsts.MaxAreaNameLength)]
        public string DocVersion { get; set; }

        public int? TenantId { get; set; }
    }
}
