using ELog.Core.Authorization;
using System.ComponentModel.DataAnnotations.Schema;

namespace ELog.Core.Entities
{
    [Table("LogoMaster")]
    public class LogoMaster : PMMSFullAudit
    {
        public string ImageTitle { get; set; }
        public byte[] ImageData { get; set; }
        public int? TenantId { get; set; }
    }
}
