using ELog.Core.Authorization;
using System.ComponentModel.DataAnnotations.Schema;

namespace ELog.Core.Entities
{
    [Table("DispensingPrintDetails")]
    public class DispensingPrintDetail : PMMSFullAudit
    {
        [ForeignKey("DispensingDetailId")]
        public int? DispensingDetailId { get; set; }

        [ForeignKey("DeviceId")]
        public int? DeviceId { get; set; }

        public bool IsController { get; set; }
    }
}