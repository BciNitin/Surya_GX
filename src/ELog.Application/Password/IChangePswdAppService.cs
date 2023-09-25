using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ELog.Application.Password.Dto;
using ELog.Application.Users.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ELog.Application.Password
{
    public interface IChangePswdAppService : IApplicationService
    {
        Task<PagedResultDto<RequestedUsersListDto>> GetAllAsync(PagedChangePasswordResultRequestDto input);
        Task<ChangePasswordDto> ChangePasswordAsync(ChangePasswordDto input);
        Task<ChangePasswordSuperAdminOutputDto> ChangePasswordForSuperAdminAsync(ChangePasswordSuperAdminDto input);
        Task<UserDto> GetAsync(EntityDto<long> input);
        Task<WMSPasswordManagerDto> CreatePasswordManagerAsync(WMSPasswordManagerDto input);

        Task<List<WMSPasswordManagerDetailsDto>> GetPasswordManagerAsync(string input);
    }
}
