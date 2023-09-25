using Abp.Authorization;
using Abp.Authorization.Users;
using Abp.Configuration;
using Abp.Configuration.Startup;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Extensions;
using Abp.UI;
using Abp.Zero.Configuration;
using ELog.Core.Authorization.Roles;
using ELog.Core.Authorization.Users;
using ELog.Core.Entities;
using ELog.Core.Identity;
using ELog.Core.MultiTenancy;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using static ELog.Core.PMMSEnums;

namespace ELog.Core.Authorization
{
    public class LogInManager : AbpLogInManager<Tenant, Role, User>
    {
        private readonly IRepository<ApprovalStatusMaster> _approvalStatusMasterRepository;
        public UserManager _userManager { get; set; }
        private readonly PMMSLoginResultTypeHelper _abpLoginResultTypeHelper;
        private readonly IRepository<User, long> _userRepository;
        public SignInManager _signInManager { get; set; }

        public LogInManager(
            UserManager userManager,
            IMultiTenancyConfig multiTenancyConfig,
            IRepository<Tenant> tenantRepository,
            IUnitOfWorkManager unitOfWorkManager, SignInManager signInManager,
            ISettingManager settingManager,
            IRepository<UserLoginAttempt, long> userLoginAttemptRepository,
            IUserManagementConfig userManagementConfig, PMMSLoginResultTypeHelper abpLoginResultTypeHelper,
            IIocResolver iocResolver,
            IPasswordHasher<User> passwordHasher,
            RoleManager roleManager, IRepository<User, long> userRepository,
            UserClaimsPrincipalFactory claimsPrincipalFactory, IRepository<Setting, long> settingRepository,
            IRepository<ApprovalStatusMaster> approvalStatusMasterRepository)
            : base(
                  userManager,
                  multiTenancyConfig,
                  tenantRepository,
                  unitOfWorkManager,
                  settingManager,
                  userLoginAttemptRepository,
                  userManagementConfig,
                  iocResolver,
                  passwordHasher,
                  roleManager,
                  claimsPrincipalFactory)
        {
            _approvalStatusMasterRepository = approvalStatusMasterRepository;
            _userManager = userManager; _abpLoginResultTypeHelper = abpLoginResultTypeHelper;
            _userRepository = userRepository;
            _signInManager = signInManager;
        }

        /// <summary>
        /// Method used for authenticating used from active directory
        /// </summary>
        /// <param name="userNameOrEmailAddress">Username or Email address of user</param>
        /// <param name="plainPassword">User entered password</param>
        /// <param name="tenancyName">Current tenant</param>
        /// <param name="shouldLockout">if wrong password whether user to be locked or not</param>
        /// <returns></returns>
        public async Task<AbpLoginResult<Tenant, User>> LoginUsingActiveDirectoryAsync(string userNameOrEmailAddress, string plainPassword, string tenancyName = null, bool shouldLockout = true)
        {
            var result = await LoginInternalUsingActiveDirectoryAsync(userNameOrEmailAddress, plainPassword, tenancyName, shouldLockout);
            await SaveLoginAttemptUsingActiveDirectoryAsync(result, tenancyName, userNameOrEmailAddress);
            return result;
        }

        /// <summary>
        /// Method used for storing all the login attempts
        /// </summary>
        /// <param name="loginResult">Contain login result whether failed or passed</param>
        /// <param name="tenancyName">Current tenantname</param>
        /// <param name="userNameOrEmailAddress">Username or email address of used authenticated</param>
        /// <returns></returns>
        private async Task SaveLoginAttemptUsingActiveDirectoryAsync(AbpLoginResult<Tenant, User> loginResult, string tenancyName, string userNameOrEmailAddress)
        {
            using (var uow = UnitOfWorkManager.Begin(TransactionScopeOption.Suppress))
            {
                var tenantId = loginResult.Tenant != null ? loginResult.Tenant.Id : (int?)null;
                using (UnitOfWorkManager.Current.SetTenantId(tenantId))
                {
                    var loginAttempt = new UserLoginAttempt
                    {
                        TenantId = tenantId,
                        TenancyName = tenancyName,

                        UserId = loginResult.User != null ? loginResult.User.Id : (long?)null,
                        UserNameOrEmailAddress = userNameOrEmailAddress,

                        Result = loginResult.Result,

                        BrowserInfo = ClientInfoProvider.BrowserInfo,
                        ClientIpAddress = ClientInfoProvider.ClientIpAddress,
                        ClientName = ClientInfoProvider.ComputerName,
                    };

                    await UserLoginAttemptRepository.InsertAsync(loginAttempt);
                    await UnitOfWorkManager.Current.SaveChangesAsync();

                    await uow.CompleteAsync();
                }
            }
        }

        /// <summary>
        /// Method used for authenticating user from active directory
        /// </summary>
        /// <param name="userNameOrEmailAddress">Username or email address of user</param>
        /// <param name="plainPassword">User entered password</param>
        /// <param name="tenancyName">Current tenantname</param>
        /// <param name="shouldLockout">if wrong password whether user to be locked or not</param>
        /// <returns></returns>
        protected async Task<AbpLoginResult<Tenant, User>> LoginInternalUsingActiveDirectoryAsync(string userNameOrEmailAddress, string plainPassword, string tenancyName, bool shouldLockout)
        {
            if (userNameOrEmailAddress.IsNullOrEmpty())
            {
                throw new ArgumentNullException(nameof(userNameOrEmailAddress));
            }

            if (plainPassword.IsNullOrEmpty())
            {
                throw new ArgumentNullException(nameof(plainPassword));
            }

            //Get and check tenant
            Tenant tenant = null;
            using (UnitOfWorkManager.Current.SetTenantId(null))
            {
                if (!MultiTenancyConfig.IsEnabled)
                {
                    tenant = await GetDefaultTenantAsync();
                }
                else if (!string.IsNullOrWhiteSpace(tenancyName))
                {
                    tenant = await TenantRepository.FirstOrDefaultAsync(t => t.TenancyName == tenancyName);
                    if (tenant == null)
                    {
                        return new AbpLoginResult<Tenant, User>(AbpLoginResultType.InvalidTenancyName);
                    }

                    if (!tenant.IsActive)
                    {
                        return new AbpLoginResult<Tenant, User>(AbpLoginResultType.TenantIsNotActive, tenant);
                    }
                }
            }

            var tenantId = tenant == null ? (int?)null : tenant.Id;
            using (UnitOfWorkManager.Current.SetTenantId(tenantId))
            {
                if (await TryLoginFromActiveDirectorySource(userNameOrEmailAddress, plainPassword, tenant))
                {
                    var user = await UserManager.FindByNameOrEmailAsync(tenantId, userNameOrEmailAddress);
                    if (user == null)
                    {
                        return new AbpLoginResult<Tenant, User>(AbpLoginResultType.InvalidUserNameOrEmailAddress, tenant);
                    }
                    if (!user.IsActive)
                    {
                        return new AbpLoginResult<Tenant, User>(AbpLoginResultType.UserIsNotActive, tenant);
                    }
                    if ((ApprovalStatus)user.ApprovalStatusId != ApprovalStatus.Approved)
                    {
                        throw new UserFriendlyException("Login Failed", "User is not approved for login into the system.");
                    }

                    if (await UserManager.IsLockedOutAsync(user))
                    {
                        return new AbpLoginResult<Tenant, User>(AbpLoginResultType.LockedOut, tenant, user);
                    }

                    return await CreateLoginResultAsync(user, tenant);
                }
                else
                {
                    return new AbpLoginResult<Tenant, User>(AbpLoginResultType.InvalidUserNameOrEmailAddress, tenant);
                }
            }
        }

        /// <summary>
        /// Method used for authenticating user from active directory
        /// </summary>
        /// <param name="userNameOrEmailAddress">Username or email address of user</param>
        /// <param name="plainPassword">User entered password</param>
        /// <param name="tenant">Current tenant</param>
        /// <returns></returns>
        protected async Task<bool> TryLoginFromActiveDirectorySource(string userNameOrEmailAddress, string plainPassword, Tenant tenant)
        {
            using (var source = IocResolver.ResolveAsDisposable<ILdapExternalAuthenticationSource>())
            {
                if (await source.Object.TryAuthenticateAsync(userNameOrEmailAddress, plainPassword, tenant))
                {
                    var tenantId = tenant == null ? (int?)null : tenant.Id;
                    using (UnitOfWorkManager.Current.SetTenantId(tenantId))
                    {
                        var user = await UserManager.FindByNameOrEmailAsync(tenantId, userNameOrEmailAddress);
                        if (user == null)
                        {
                            user = await source.Object.CreateUserAsync(userNameOrEmailAddress, tenant);

                            user.TenantId = tenantId;
                            user.AuthenticationSource = source.Object.Name;
                            user.Password = UserManager.PasswordHasher.HashPassword(user, Guid.NewGuid().ToString("N").Left(16)); //Setting a random password since it will not be used
                            user.SetNormalizedNames();
                            user.ApprovalStatusId = (int)ApprovalStatus.Approved;
                            //if (user.Roles == null)
                            //{
                            //    user.Roles = new List<UserRole>();
                            //    foreach (var defaultRole in RoleManager.Roles.Where(r => r.TenantId == tenantId && r.IsDefault).ToList())
                            //    {
                            //        user.Roles.Add(new UserRole(tenantId, user.Id, defaultRole.Id));
                            //    }
                            //}

                            await UserManager.CreateAsync(user);
                        }
                        else
                        {
                            await source.Object.UpdateUserAsync(user, tenant);

                            user.AuthenticationSource = source.Object.Name;

                            await UserManager.UpdateAsync(user);
                        }

                        await UnitOfWorkManager.Current.SaveChangesAsync();

                        return true;
                    }
                }
            }

            return false;
        }

        protected override async Task<AbpLoginResult<Tenant, User>> LoginAsyncInternal(string userNameOrEmailAddress, string plainPassword, string tenancyName, bool shouldLockout)
        {
            if (userNameOrEmailAddress.IsNullOrEmpty())
            {
                throw new ArgumentNullException(nameof(userNameOrEmailAddress));
            }

            if (plainPassword.IsNullOrEmpty())
            {
                throw new ArgumentNullException(nameof(plainPassword));
            }

            //Get and check tenant
            Tenant tenant = null;
            using (UnitOfWorkManager.Current.SetTenantId(null))
            {
                if (!MultiTenancyConfig.IsEnabled)
                {
                    tenant = await GetDefaultTenantAsync();
                }
                else if (!string.IsNullOrWhiteSpace(tenancyName))
                {
                    tenant = await TenantRepository.FirstOrDefaultAsync(t => t.TenancyName == tenancyName);
                    if (tenant == null)
                    {
                        return new AbpLoginResult<Tenant, User>(AbpLoginResultType.InvalidTenancyName);
                    }

                    if (!tenant.IsActive)
                    {
                        return new AbpLoginResult<Tenant, User>(AbpLoginResultType.TenantIsNotActive, tenant);
                    }
                }
            }
            var tenantId = tenant == null ? (int?)null : tenant.Id;
            using (UnitOfWorkManager.Current.SetTenantId(tenantId))
            {
                //TryLoginFromExternalAuthenticationSources method may create the user, that's why we are calling it before AbpStore.FindByNameOrEmailAsync
                var loggedInFromExternalSource = await TryLoginFromExternalAuthenticationSources(userNameOrEmailAddress, plainPassword, tenant);

                var user = await UserManager.FindByNameOrEmailAsync(tenantId, userNameOrEmailAddress);
                if (user == null)
                {
                    return new AbpLoginResult<Tenant, User>(AbpLoginResultType.InvalidUserNameOrEmailAddress, tenant);
                }
                var resetPasswordDaysLeft = await _userManager.GetResetPasswordDaysLeft(user.Id);
                if (resetPasswordDaysLeft <= 0)
                {
                    throw _abpLoginResultTypeHelper.CreateExceptionForFailedLoginAttempt(null, resetPasswordDaysLeft);
                }
                if (!loggedInFromExternalSource)
                {
                    await _userManager.InitializeLockoutSettings(tenantId);
                    bool wrongPassword = false;
                    bool wrongsccessRehashPassword = false;
                    var verificationResult = UserManager.PasswordHasher.VerifyHashedPassword(user, user.Password, plainPassword);
                    if (verificationResult == PasswordVerificationResult.Failed)
                    {
                        wrongPassword = true;
                    }
                    if (verificationResult == PasswordVerificationResult.SuccessRehashNeeded)
                    {
                        wrongsccessRehashPassword = true;
                    }
                    if (user.IsLockout)
                    {
                        return new AbpLoginResult<Tenant, User>(AbpLoginResultType.LockedOut, tenant, user);
                    }
                    if (wrongPassword || wrongsccessRehashPassword)
                    {
                        using (var uow = UnitOfWorkManager.Begin(TransactionScopeOption.Suppress))
                        {
                            using (UnitOfWorkManager.Current.SetTenantId(tenantId))
                            {
                                await _userManager.IncrementAccessFailedCountAsync(user);

                                await UnitOfWorkManager.Current.SaveChangesAsync();

                                await uow.CompleteAsync();
                            }
                        }
                        var accessFailedCount = await _userManager.GetAccessFailedCountAsync(user);
                        var attemptsLeft = Convert.ToInt32(_userManager.MaxFailedAccessAttemptsBeforeLockout) - accessFailedCount;
                        if (attemptsLeft <= 0)
                        {
                            using (var uow = UnitOfWorkManager.Begin(TransactionScopeOption.Suppress))
                            {
                                using (UnitOfWorkManager.Current.SetTenantId(tenantId))
                                {

                                    await _userManager.UpdateLockedOutSettingAsync(user);

                                    await UnitOfWorkManager.Current.SaveChangesAsync();

                                    await uow.CompleteAsync();
                                }
                            }
                            shouldLockout = true;
                            return new AbpLoginResult<Tenant, User>(AbpLoginResultType.LockedOut, tenant, user);
                        }
                        else
                        {
                            return await GetFailedPasswordValidationAsLoginResultAsync(user, tenant, shouldLockout, attemptsLeft);
                        }
                    }
                    if ((ApprovalStatus)user.ApprovalStatusId != ApprovalStatus.Approved)
                    {
                        throw new UserFriendlyException("Login Failed", "User is not approved for login into the system.");
                    }
                    await _userManager.ResetAccessFailedCountAsync(user);
                }

                return await CreateLoginResultAsync(user, tenant);
            }
        }

        protected virtual async Task<bool> TryLoginFromExternalAuthenticationSources(string userNameOrEmailAddress, string plainPassword, Tenant tenant)
        {
            if (!UserManagementConfig.ExternalAuthenticationSources.Any())
            {
                return false;
            }

            foreach (var sourceType in UserManagementConfig.ExternalAuthenticationSources)
            {
                using (var source = IocResolver.ResolveAsDisposable<IExternalAuthenticationSource<Tenant, User>>(sourceType))
                {
                    if (await source.Object.TryAuthenticateAsync(userNameOrEmailAddress, plainPassword, tenant))
                    {
                        var tenantId = tenant == null ? (int?)null : tenant.Id;
                        using (UnitOfWorkManager.Current.SetTenantId(tenantId))
                        {
                            var user = await UserManager.FindByNameOrEmailAsync(tenantId, userNameOrEmailAddress);
                            if (user == null)
                            {
                                user = await source.Object.CreateUserAsync(userNameOrEmailAddress, tenant);

                                user.TenantId = tenantId;
                                user.AuthenticationSource = source.Object.Name;
                                user.Password = UserManager.PasswordHasher.HashPassword(user, Guid.NewGuid().ToString("N").Left(16)); //Setting a random password since it will not be used
                                user.SetNormalizedNames();

                                if (user.Roles == null)
                                {
                                    user.Roles = new List<UserRole>();
                                    foreach (var defaultRole in RoleManager.Roles.Where(r => r.TenantId == tenantId && r.IsDefault).ToList())
                                    {
                                        user.Roles.Add(new UserRole(tenantId, user.Id, defaultRole.Id));
                                    }
                                }

                                await UserManager.CreateAsync(user);
                            }
                            else
                            {
                                await source.Object.UpdateUserAsync(user, tenant);

                                user.AuthenticationSource = source.Object.Name;

                                await UserManager.UpdateAsync(user);
                            }

                            await UnitOfWorkManager.Current.SaveChangesAsync();

                            return true;
                        }
                    }
                }
            }

            return false;
        }

        protected virtual async Task<AbpLoginResult<Tenant, User>> GetFailedPasswordValidationAsLoginResultAsync(User user, Tenant tenant = null, bool shouldLockout = false, int userLoginAttemptsLeft = 0)
        {
            if (shouldLockout)
            {
                throw _abpLoginResultTypeHelper.CreateExceptionForFailedLoginAttempt(userLoginAttemptsLeft, null);
            }

            return new AbpLoginResult<Tenant, User>(AbpLoginResultType.InvalidPassword, tenant, user);
        }

        protected virtual async Task<AbpLoginResult<Tenant, User>> GetSuccessRehashNeededAsLoginResultAsync(User user, Tenant tenant = null, bool shouldLockout = false, int userLoginAttemptsLeft = 0)
        {
            return await GetFailedPasswordValidationAsLoginResultAsync(user, tenant, shouldLockout, userLoginAttemptsLeft);
        }
    }
}