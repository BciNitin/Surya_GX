using AutoMapper;
using ELog.Core.Entities;

namespace ELog.Application.Notification.Dto
{
    public class NotificationMapProfile : Profile
    {
        public NotificationMapProfile()
        {
            CreateMap<NotificationDto, Notifications>();
            CreateMap<NotificationDto, Notifications>();
            CreateMap<CreateNotificationDto, Notifications>();

            CreateMap<NotificationDto, Notifications>();
            CreateMap<NotificationDto, NotificationDto>();
        }
    }
}
