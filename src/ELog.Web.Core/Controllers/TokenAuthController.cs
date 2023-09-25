using Abp.Authorization;
using Abp.Authorization.Users;
using Abp.MultiTenancy;
using Abp.Runtime.Security;
using Abp.UI;
using ELog.Application;
using ELog.Application.Authorization;
using ELog.Core;
using ELog.Core.Authorization;
using ELog.Core.Authorization.Users;
using ELog.Core.MultiTenancy;
using ELog.Web.Core.Authentication.External;
using ELog.Web.Core.Authentication.JwtBearer;
using ELog.Web.Core.Models.TokenAuth;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AuthUser = ELog.Core.Authorization.Users;

namespace ELog.Web.Core.Controllers
{
    [Route("api/[controller]/[action]")]
    public class TokenAuthController : PMMSControllerBase
    {
        private readonly LogInManager _logInManager;
        private readonly ITenantCache _tenantCache;
        private readonly AbpLoginResultTypeHelper _abpLoginResultTypeHelper;
        private readonly TokenAuthConfiguration _configuration;
        private readonly IExternalAuthConfiguration _externalAuthConfiguration;
        private readonly IExternalAuthManager _externalAuthManager;
        private readonly UserRegistrationManager _userRegistrationManager;
        private readonly UserManager _userManager;

        public TokenAuthController(
            LogInManager logInManager,
            ITenantCache tenantCache,
            AbpLoginResultTypeHelper abpLoginResultTypeHelper,
            TokenAuthConfiguration configuration,
            IExternalAuthConfiguration externalAuthConfiguration,
            IExternalAuthManager externalAuthManager,
            UserRegistrationManager userRegistrationManager,
            UserManager userManager)
        {
            _logInManager = logInManager;
            _tenantCache = tenantCache;
            _abpLoginResultTypeHelper = abpLoginResultTypeHelper;
            _configuration = configuration;
            _externalAuthConfiguration = externalAuthConfiguration;
            _externalAuthManager = externalAuthManager;
            _userRegistrationManager = userRegistrationManager;
            _userManager = userManager;
        }

        /// <summary>
        /// Method used for authenticating user from database and return access token.
        /// </summary>
        /// <param name="model">This model contain usernameoremailaddress,password for authentication</param>
        /// <returns>AuthenticateResultModel : Contains access token,user id and token expiration</returns>
        [HttpPost]
        public async Task<AuthenticateResultModel> Authenticate([FromBody] AuthenticateModel model)
        {
            try
            {
                var loginResult = await GetLoginResultAsync(
             model.UserNameOrEmailAddress,
             model.Password,
             GetTenancyNameOrNull()
         );

                var accessToken = CreateAccessToken(CreateJwtClaims(loginResult.Identity));
                var userPlants = await _userManager.GetUsersPlantList(loginResult.User);
                await _userManager.UpdateAsync(loginResult.User);
                var refreshToken = EncryptDecryptHelper.Encrypt(await _userManager.GetConcurrencyStamp(loginResult.User.Id));
                return new AuthenticateResultModel
                {
                    AccessToken = accessToken,
                    EncryptedAccessToken = GetEncryptedAccessToken(accessToken),
                    ExpireInSeconds = (int)_configuration.Expiration.TotalSeconds,
                    UserId = loginResult.User.Id,
                    isMultiplePlantExists = userPlants.Count() > 1,
                    plantId = userPlants.FirstOrDefault(),
                    RefreshToken = refreshToken,
                    PasswordStatus = loginResult.User.PasswordStatus
                };
            }
            catch (Exception)
            {
                return null;
            }



        }

        /// <summary>
        /// Method used for authenticating user from active directory and return access token.
        /// </summary>
        /// <param name="model">This model contain usernameoremailaddress,password for authentication</param>
        /// <returns>AuthenticateResultModel : Contains access token,user id and token expiration</returns>
        [HttpPost]
        public async Task<AuthenticateResultModel> AuthenticateUsingActiveDirectory([FromBody] AuthenticateModel model)
        {
            var loginResult = await GetLoginResultUsingActiveDirectoryAsync(
                model.UserNameOrEmailAddress,
                model.Password,
                GetTenancyNameOrNull()
            );

            string accessToken = CreateAccessToken(CreateJwtClaims(loginResult.Identity));
            var usersPlants = await _userManager.GetUsersPlantList(loginResult.User);
            return new AuthenticateResultModel
            {
                AccessToken = accessToken,
                EncryptedAccessToken = GetEncryptedAccessToken(accessToken),
                ExpireInSeconds = (int)_configuration.Expiration.TotalSeconds,
                UserId = loginResult.User.Id,
                isMultiplePlantExists = usersPlants.Count() > 1,
                plantId = usersPlants.FirstOrDefault(),
            };
        }

        [HttpGet]
        public List<ExternalLoginProviderInfoModel> GetExternalAuthenticationProviders()
        {
            return ObjectMapper.Map<List<ExternalLoginProviderInfoModel>>(_externalAuthConfiguration.Providers);
        }

        [HttpPost]
        public async Task<ExternalAuthenticateResultModel> ExternalAuthenticate([FromBody] ExternalAuthenticateModel model)
        {
            var externalUser = await GetExternalUserInfo(model);
            var loginResult = await _logInManager.LoginAsync(new UserLoginInfo(model.AuthProvider, model.ProviderKey, model.AuthProvider), GetTenancyNameOrNull());

            switch (loginResult.Result)
            {
                case AbpLoginResultType.Success:
                    {
                        var accessToken = CreateAccessToken(CreateJwtClaims(loginResult.Identity));
                        return new ExternalAuthenticateResultModel
                        {
                            AccessToken = accessToken,
                            EncryptedAccessToken = GetEncryptedAccessToken(accessToken),
                            ExpireInSeconds = (int)_configuration.Expiration.TotalSeconds
                        };
                    }
                case AbpLoginResultType.UnknownExternalLogin:
                    {
                        var newUser = await RegisterExternalUserAsync(externalUser);

                        if (!newUser.IsActive)
                        {
                            return new ExternalAuthenticateResultModel
                            {
                                WaitingForActivation = true
                            };
                        }

                        // Try to login again with newly registered user!
                        loginResult = await _logInManager.LoginAsync(new UserLoginInfo(model.AuthProvider, model.ProviderKey, model.AuthProvider), GetTenancyNameOrNull());

                        if (loginResult.Result != AbpLoginResultType.Success)
                        {
                            throw _abpLoginResultTypeHelper.CreateExceptionForFailedLoginAttempt(
                                loginResult.Result,
                                model.ProviderKey,
                                GetTenancyNameOrNull()
                            );
                        }

                        return new ExternalAuthenticateResultModel
                        {
                            AccessToken = CreateAccessToken(CreateJwtClaims(loginResult.Identity)),
                            ExpireInSeconds = (int)_configuration.Expiration.TotalSeconds
                        };
                    }
                default:
                    {
                        throw _abpLoginResultTypeHelper.CreateExceptionForFailedLoginAttempt(
                            loginResult.Result,
                            model.ProviderKey,
                            GetTenancyNameOrNull()
                        );
                    }
            }
        }

        private async Task<User> RegisterExternalUserAsync(ExternalAuthUserInfo externalUser)
        {
            var user = await _userRegistrationManager.RegisterExternalAsync(
                externalUser.Name,
                externalUser.Surname,
                externalUser.EmailAddress,
                externalUser.EmailAddress,
                AuthUser.User.CreateRandomPassword(),
                isEmailConfirmed: true
            );

            user.Logins = new List<UserLogin>
            {
                new UserLogin
                {
                    LoginProvider = externalUser.Provider,
                    ProviderKey = externalUser.ProviderKey,
                    TenantId = user.TenantId
                }
            };

            await CurrentUnitOfWork.SaveChangesAsync();

            return user;
        }

        private async Task<ExternalAuthUserInfo> GetExternalUserInfo(ExternalAuthenticateModel model)
        {
            var userInfo = await _externalAuthManager.GetUserInfo(model.AuthProvider, model.ProviderAccessCode);
            if (userInfo.ProviderKey != model.ProviderKey)
            {
                throw new UserFriendlyException(L("CouldNotValidateExternalUser"));
            }
            return userInfo;
        }

        private string GetTenancyNameOrNull()
        {
            if (!AbpSession.TenantId.HasValue)
            {
                return null;
            }

            return _tenantCache.GetOrNull(AbpSession.TenantId.Value)?.TenancyName;
        }

        /// <summary>
        /// Method used for authenticating user information from database and return authentication result whether failed or passed
        /// </summary>
        /// <param name="usernameOrEmailAddress">Contains unique username or email address</param>
        /// <param name="password">Contain user password</param>
        /// <param name="tenancyName">Current tenant</param>
        /// <returns></returns>
        private async Task<AbpLoginResult<Tenant, User>> GetLoginResultAsync(string usernameOrEmailAddress, string password, string tenancyName)
        {
            var loginResult = await _logInManager.LoginAsync(usernameOrEmailAddress, password, tenancyName);

            switch (loginResult.Result)
            {
                case AbpLoginResultType.Success:
                    return loginResult;

                default:
                    throw _abpLoginResultTypeHelper.CreateExceptionForFailedLoginAttempt(loginResult.Result, usernameOrEmailAddress, tenancyName);
            }
        }

        /// <summary>
        /// Method used for authenticating user information from active directory and return authentication result whether failed or passed
        /// </summary>
        /// <param name="usernameOrEmailAddress">Contains unique username or email address</param>
        /// <param name="password">Contain user password</param>
        /// <param name="tenancyName">Current tenant</param>
        private async Task<AbpLoginResult<Tenant, User>> GetLoginResultUsingActiveDirectoryAsync(string usernameOrEmailAddress, string password, string tenancyName)
        {
            var loginResult = await _logInManager.LoginUsingActiveDirectoryAsync(usernameOrEmailAddress, password, tenancyName);
            var adminorSupderAdminCredentials = new List<string> { PMMSConsts.SuperAdminUserName, PMMSConsts.TenantAdminEmailAddress, AbpUserBase.AdminUserName };
            if (adminorSupderAdminCredentials.Contains(usernameOrEmailAddress))
            {
                loginResult = await _logInManager.LoginAsync(usernameOrEmailAddress, password, tenancyName);
            }
            switch (loginResult.Result)
            {
                case AbpLoginResultType.Success:
                    return loginResult;

                default:
                    throw _abpLoginResultTypeHelper.CreateExceptionForFailedLoginAttempt(loginResult.Result, usernameOrEmailAddress, tenancyName);
            }
        }

        /// <summary>
        /// Method used for creating access token from list of user claims information from identity
        /// </summary>
        /// <param name="claims">List of user claim informations</param>
        /// <param name="expiration">Optional parameter contains expiration time to be assigned while token creation</param>
        /// <returns>string : Contains JWT access token</returns>
        private string CreateAccessToken(IEnumerable<Claim> claims, TimeSpan? expiration = null)
        {
            var now = DateTime.UtcNow;

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _configuration.Issuer,
                audience: _configuration.Audience,
                claims: claims,
                notBefore: now.AddDays(-1),
                expires: now.Add(expiration ?? _configuration.Expiration),
                signingCredentials: _configuration.SigningCredentials
            );

            return new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
        }

        /// <summary>
        /// Method used for creating claims from identity
        /// </summary>
        /// <param name="identity">identity of current user</param>
        /// <returns>List of Claim : Contains list of user claim information from identity</returns>
        private static List<Claim> CreateJwtClaims(ClaimsIdentity identity)
        {
            var claims = identity.Claims.ToList();
            var nameIdClaim = claims.First(c => c.Type == ClaimTypes.NameIdentifier);

            // Specifically add the jti (random nonce), iat (issued timestamp), and sub (subject/user) claims.
            claims.AddRange(new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, nameIdClaim.Value),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.Now.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
            });

            return claims;
        }

        /// <summary>
        /// Method encrypt the access token and return encrypted access token
        /// </summary>
        /// <param name="accessToken">string : Contain encrypted access token</param>
        /// <returns></returns>
        private string GetEncryptedAccessToken(string accessToken)
        {
            return SimpleStringCipher.Instance.Encrypt(accessToken, AppConsts.DefaultPassPhrase);
        }

        [HttpPost]
        public async Task<AuthenticateResultModel> RefreshToken(string refreshToken)
        {
            var decryptedToken = EncryptDecryptHelper.Decrypt(refreshToken);
            var dbRefreshToken = await _userManager.GetConcurrencyStamp(AbpSession.UserId.GetValueOrDefault());
            if (dbRefreshToken != decryptedToken)
            {
                throw new UserFriendlyException("Another session is active for user.  Please login again to proceed.");
            }
            var userPlants = await _userManager.GetUserPlantList(AbpSession.UserId.GetValueOrDefault());
            var accessToken = CreateAccessToken(User.Claims);
            return new AuthenticateResultModel
            {
                AccessToken = accessToken,
                EncryptedAccessToken = GetEncryptedAccessToken(accessToken),
                ExpireInSeconds = (int)_configuration.Expiration.TotalSeconds,
                UserId = AbpSession.UserId.GetValueOrDefault(),
                isMultiplePlantExists = userPlants.Count() > 1,
                plantId = userPlants.FirstOrDefault(),
                RefreshToken = EncryptDecryptHelper.Encrypt(dbRefreshToken)
            };
        }
    }
}