using Abp.Domain.Entities;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ELog.Core.Entities
{
    [Table("Notification")]
    public class Notifications : Entity<int>
    {
        public int Id { get; set; }
        public int notification_type { get; set; }
        public string assign_roles { get; set; }
        public int assign_email { get; set; }
        public int assign_mobile { get; set; }

        public int log_Id { get; set; }
        public bool isActive { get; set; }
        public int Repeat { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
    }
}
