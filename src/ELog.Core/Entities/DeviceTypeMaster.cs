using ELog.Core.Authorization;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ELog.Core.Entities
{
    [Table("DeviceTypeMaster")]
    public class DeviceTypeMaster : PMMSFullAudit
    {
        [Required]
        public string DeviceName { get; set; }

        [ForeignKey("DeviceTypeId")]
        public ICollection<DeviceMaster> DeviceMasters { get; set; }
    }
}