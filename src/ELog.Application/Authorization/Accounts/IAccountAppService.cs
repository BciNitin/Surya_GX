using Abp.Application.Services;

using ELog.Application.Authorization.Accounts.Dto;
using ELog.Application.Users.Dto;
using System.Threading.Tasks;

namespace ELog.Application.Authorization.Accounts
{
    public interface IAccountAppService : IApplicationService
    {
        Task<IsTenantAvailableOutput> IsTenantAvailable(IsTenantAvailableInput input);
        Task<RegisterOutput> Register(RegisterInput input);
        Task<ForgotPasswordOutput> ForgotPasswordAsync(ForgotPasswordDto input, bool isForgotPswd);
    }
}
