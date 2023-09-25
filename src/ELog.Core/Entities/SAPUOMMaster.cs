using ELog.Core.Authorization;

using System.ComponentModel.DataAnnotations.Schema;

namespace ELog.Core.Entities
{
    [Table("SAPUOMMaster")]
    public class SAPUOMMaster : PMMSFullAudit
    {
        public string UOM { get; set; }
        public string Description { get; set; }
    }
}