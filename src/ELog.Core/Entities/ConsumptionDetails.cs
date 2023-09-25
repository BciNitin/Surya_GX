using ELog.Core.Authorization;
using System.ComponentModel.DataAnnotations.Schema;

namespace ELog.Core.Entities
{
    [Table("ConsumptionDetails")]
    public class ConsumptionDetails : PMMSFullAudit
    {
        [ForeignKey("ConsumptionId")]
        public int? ConsumptionId { get; set; }
        public int? MaterialBarocdeId { get; set; }
        public string LineItemNo { get; set; }

        public string BatchNo { get; set; }

    }
}
