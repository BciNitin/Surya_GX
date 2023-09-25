using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ELog.Application.Notification.Dto;
using System.Threading.Tasks;

namespace ELog.Application.Notification
{
    public interface INotificationService : IApplicationService
    {
        Task<NotificationDto> CreateAsync(NotificationDto input);

        //Task<NotificationDto> GetAsync(EntityDto<int> input);
        Task<PagedResultDto<NotificationDto>> GetAllAsync(PagedNotificationResultRequestDto input);
    }
}
