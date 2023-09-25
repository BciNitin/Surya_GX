using ELog.Core.Authorization;
using System.ComponentModel.DataAnnotations.Schema;

namespace ELog.Core.Entities
{
    [Table("Putaway")]
    public class Putaway : PMMSFullAudit
    {
        public int LocationId { get; set; }
        public int ContainerId { get; set; }
        public string ContainerCode { get; set; }
        public bool isActive { get; set; }

        public int ProductCodeId { get; set; }
        public string StorageLocation { get; set; }
        public string ProcessOrderNo { get; set; }
        public int ProcessOrderId { get; set; }




    }
}
