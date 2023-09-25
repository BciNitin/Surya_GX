using ELog.Core.Authorization;
using System.ComponentModel.DataAnnotations.Schema;

namespace ELog.Core.Entities
{
    [Table("ReportConfiguration")]
    public class ReportConfiguration : PMMSFullAudit
    {
        public long? UserId { get; set; }
        [ForeignKey("SubModuleId")]
        public int SubModuleId { get; set; }
        public string ReportSettings { get; set; }
    }
}
