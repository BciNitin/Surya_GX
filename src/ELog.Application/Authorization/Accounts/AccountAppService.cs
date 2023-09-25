using Abp.Configuration;
using Abp.UI;
using Abp.Zero.Configuration;

using ELog.Application.Authorization.Accounts.Dto;
using ELog.Application.Users.Dto;
using ELog.Core;
using ELog.Core.Authorization.Users;

using System.Threading.Tasks;
using static ELog.Core.PMMSEnums;

namespace ELog.Application.Authorization.Accounts
{
    public class AccountAppService : PMMSAppServiceBase, IAccountAppService
    {
        // from: http://regexlib.com/REDetails.aspx?regexp_id=1923
        public const string PasswordRegex = "(?=^.{8,}$)(?=.*\\d)(?=.*[a-z])(?=.*[A-Z])(?!.*\\s)[0-9a-zA-Z!@#$%^&*()]*$";

        private readonly UserRegistrationManager _userRegistrationManager;
        private readonly UserManager _userManager;
        public AccountAppService(
            UserRegistrationManager userRegistrationManager, UserManager userManager)
        {
            _userManager = userManager;
            _userRegistrationManager = userRegistrationManager;

        }

        public async Task<IsTenantAvailableOutput> IsTenantAvailable(IsTenantAvailableInput input)
        {
            var tenant = await TenantManager.FindByTenancyNameAsync(input.TenancyName);
            if (tenant == null)
            {
                return new IsTenantAvailableOutput(TenantAvailabilityState.NotFound);
            }

            if (!tenant.IsActive)
            {
                return new IsTenantAvailableOutput(TenantAvailabilityState.InActive);
            }

            return new IsTenantAvailableOutput(TenantAvailabilityState.Available, tenant.Id);
        }

        public async Task<RegisterOutput> Register(RegisterInput input)
        {
            var user = await _userRegistrationManager.RegisterAsync(
                input.Name,
                input.Surname,
                input.EmailAddress,
                input.UserName,
                input.Password,
                isEmailConfirmed: true // Assumed email address is always confirmed. Change this if you want to implement email confirmation.
            );

            var isEmailConfirmationRequiredForLogin = await SettingManager.GetSettingValueAsync<bool>(AbpZeroSettingNames.UserManagement.IsEmailConfirmationRequiredForLogin);

            return new RegisterOutput
            {
                CanLogin = user.IsActive && (user.IsEmailConfirmed || !isEmailConfirmationRequiredForLogin)
            };
        }


        public async Task<ForgotPasswordOutput> ForgotPasswordAsync(ForgotPasswordDto input, bool isForgotPswd)
        {
            return await ForgotPassword(input, isForgotPswd).ConfigureAwait(false);
        }

        private async Task<ForgotPasswordOutput> ForgotPassword(ForgotPasswordDto input, bool isForgotPswd)
        {
            var user = await _userManager.FindByNameAsync(input.EmployeeCode);
            const int approvalStatusValue = (int)(PMMSEnums.ApprovalStatus.Approved);


            if (user != null && user.IsActive && user.ApprovalStatusId == approvalStatusValue && !user.IsDeleted)
            {

                var rolesList = await _userManager.GetRolesAsync(user);
                var userRole = string.Empty;

                try
                {
                    if (isForgotPswd)
                    {
                        if (rolesList.Contains(RoleCategories.SuperAdmin.ToString()))
                        {
                            userRole = RoleCategories.SuperAdmin.ToString();
                        }

                        return new ForgotPasswordOutput
                        {
                            Result = true,
                            UserRole = userRole
                        };

                    }
                    else
                    {

                        if (rolesList.Contains(RoleCategories.SuperAdmin.ToString()))
                        {
                            userRole = RoleCategories.SuperAdmin.ToString();
                        }
                        else if (rolesList.Contains(RoleCategories.Admin.ToString()))
                        {
                            user.PasswordStatus = (int)ResetPasswordStatus.ResetPending;
                            userRole = RoleCategories.Admin.ToString();
                            CheckErrors(await _userManager.UpdateAsync(user).ConfigureAwait(false));
                        }
                        else
                        {
                            user.PasswordStatus = (int)ResetPasswordStatus.ResetPending;
                            userRole = "user";
                            CheckErrors(await _userManager.UpdateAsync(user).ConfigureAwait(false));
                        }

                        return new ForgotPasswordOutput
                        {
                            Result = true,
                            UserRole = userRole
                        };
                    }
                }
                catch (UserFriendlyException ex)
                {
                    if (ex.Message.Contains("User name"))
                    {
                        string newMessage = ex.Message.Replace("User name", "Username");
                        throw new UserFriendlyException(422, newMessage);
                    }
                }
            }
            else
            {
                return new ForgotPasswordOutput
                {
                    Result = false
                };
            }
            return new ForgotPasswordOutput
            {
                Result = true
            };
        }
    }
}
