using Abp.Application.Services;
using Abp.Application.Services.Dto;

using ELog.Application.Users.Dto;

using System.Threading.Tasks;

namespace ELog.Application.Users
{
    public interface IUserAppService : IApplicationService
    {
        Task<UserDto> GetAsync(EntityDto<long> input);

        Task<PagedResultDto<UsersListDto>> GetAllAsync(PagedUserResultRequestDto input);

        Task<UserDto> CreateAsync(CreateUserDto input);

        Task<UserDto> UpdateAsync(UserDto input);

        Task DeleteAsync(EntityDto<long> input);

        Task<RoleCheckboxDto> GetAllRoles();
        Task<UserDto> GetUserProfileAsync(EntityDto<long> input);
        Task AddOrUpdateUserCreationLimitAsync(long noOfUsers);
    }
}