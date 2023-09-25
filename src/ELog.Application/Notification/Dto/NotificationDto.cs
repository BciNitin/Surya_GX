using Abp.AutoMapper;
using ELog.Core.Entities;
using System;

namespace ELog.Application.Notification.Dto
{
    [AutoMapFrom(typeof(Notifications))]
    public class NotificationDto
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



