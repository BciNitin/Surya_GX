using ELog.Core.Authorization;
using ELog.Core.Authorization.Users;

using System.ComponentModel.DataAnnotations.Schema;

namespace ELog.Core.Entities
{
    [Table("UserPlants")]
    public class UserPlants : PMMSFullAudit
    {
        public long UserId { get; set; }

        public int PlantId { get; set; }

        public int? TenantId { get; set; }

        public PlantMaster PlantMaster { get; set; }
        public User User { get; set; }
    }
}