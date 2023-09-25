using Abp.Application.Services;

using ELog.Application.Sessions.Dto;

using System.Threading.Tasks;

namespace ELog.Application.Sessions
{
    public interface ISessionAppService : IApplicationService
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations();
    }
}
