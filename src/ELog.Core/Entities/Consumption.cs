using ELog.Core.Authorization;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ELog.Core.Entities
{
    [Table("Consumption")]
    public class Consumption : PMMSFullAudit
    {
        [ForeignKey("CubicleId")]
        public int? CubicleId { get; set; }
        public int? ProductId { get; set; }

        [ForeignKey("ProcessOrderId")]
        public int? ProcessOrderId { get; set; }

        [ForeignKey("EquipmentId")]
        public int? EquipmentId { get; set; }

        public int? NoOfContainer { get; set; }

        [ForeignKey("ConsumptionId")]
        public ICollection<ConsumptionDetails> ConsumptionDetails { get; set; }

    }
}
