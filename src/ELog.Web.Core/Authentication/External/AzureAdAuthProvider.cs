using Abp.Localization;
using Abp.Localization.Sources;
using Abp.UI;
using ELog.Core;
using ELog.Web.Core.Models.TokenAuth;
using Microsoft.Extensions.Configuration;
using Microsoft.Graph;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ELog.Web.Core.Authentication.External
{
    public class AzureAdAuthProvider : ExternalAuthProviderApiBase
    {
        public const string Name = "AzureAd";
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILocalizationManager _localizationManager;

        public AzureAdAuthProvider(IConfiguration configuration, IHttpClientFactory httpClientFactory, ILocalizationManager localizationManager)
        {
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
            _localizationManager = localizationManager;
        }

        public override async Task<ExternalAuthUserInfo> GetUserInfo(string accessCode)
        {
            var source = _localizationManager.GetSource(PMMSConsts.LocalizationSourceName);

            string stsDiscoveryEndpoint = $"https://login.microsoftonline.com/{_configuration["Authentication:AzureAd:TenantId"]}/v2.0/.well-known/openid-configuration";

            var configManager = new ConfigurationManager<OpenIdConnectConfiguration>(stsDiscoveryEndpoint, new OpenIdConnectConfigurationRetriever());

            var config = await configManager.GetConfigurationAsync();

            var accessTokenResponse = await GetAccessTokenAsync(source);

            var tokenValidationResult = ValidaAccessCode(accessCode, config);

            if (!tokenValidationResult.IsValid)
            {
                throw new UserFriendlyException(source.GetString("CouldNotValidateExternalUser"));
            }

            var user = await GetUserInformationasync(source, accessTokenResponse, tokenValidationResult);

            var fullname = user.DisplayName.Split(' ');

            return new ExternalAuthUserInfo
            {
                Name = string.IsNullOrEmpty(user.GivenName) ? fullname[0] : user.GivenName,
                EmailAddress = user.Mail,
                Surname = string.IsNullOrEmpty(user.Surname) ? fullname[1] : user.Surname,
                Provider = Name,
                ProviderKey = user.Id
            };
        }

        private static async Task<User> GetUserInformationasync(ILocalizationSource source, AzureAdAccessTokenResponse accessTokenResponse, AccessCodeValidationResponse tokenValidationResult)
        {
            var graphClient = new GraphServiceClient(new DelegateAuthenticationProvider(
                            async requestMessage =>
                            {
                                // Append the access token to the request
                                requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessTokenResponse.AccessToken);
                            }));


            var users = await graphClient.Users.Request().GetAsync();

            var user = users?.FirstOrDefault(c => c.Mail == tokenValidationResult.Email || c.GivenName == tokenValidationResult.Email);

            if (user == null)
            {
                throw new UserFriendlyException(source.GetString("CouldNotValidateExternalUser"));
            }

            return user;
        }

        private AccessCodeValidationResponse ValidaAccessCode(string accessCode, OpenIdConnectConfiguration config)
        {
            var result = new AccessCodeValidationResponse();

            try
            {
                var handler = new JwtSecurityTokenHandler();

                if (!handler.CanValidateToken)
                {
                    result.IsValid = false;
                    return result;
                }

                var claims = handler.ValidateToken(accessCode, new TokenValidationParameters
                {
                    ValidateAudience = true,
                    ValidAudiences = new[] { _configuration["Authentication:AzureAd:ClientId"] },
                    ValidateLifetime = true,
                    IssuerSigningKeys = config.SigningKeys,
                    ValidateIssuer = false,
                }, out SecurityToken securityToken);


                result.UserId = claims.FindFirst(claim => claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier")?.Value;
                result.Email = claims.FindFirst(claim => claim.Type == ClaimTypes.Email)?.Value;
            }
            catch (System.Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                result.IsValid = false;
            }
            return result;
        }

        private async Task<AzureAdAccessTokenResponse> GetAccessTokenAsync(Abp.Localization.Sources.ILocalizationSource source)
        {
            var client = _httpClientFactory.CreateClient();

            var keyValues = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("client_id", _configuration["Authentication:AzureAd:ClientId"]),
                new KeyValuePair<string, string>("client_secret", _configuration["Authentication:AzureAd:ClientSecret"]),
                new KeyValuePair<string, string>("resource", "https://graph.microsoft.com"),
                new KeyValuePair<string, string>("grant_type", "client_credentials")
            };

            var response = await client.PostAsync($"https://login.microsoftonline.com/{_configuration["Authentication:AzureAd:TenantId"]}/oauth2/token", new FormUrlEncodedContent(keyValues));

            response.EnsureSuccessStatusCode();

            var contentResult = await response.Content.ReadAsStringAsync();

            if (string.IsNullOrEmpty(contentResult))
            {
                throw new UserFriendlyException(source.GetString("CouldNotValidateExternalUser"));
            }

            return JsonConvert.DeserializeObject<AzureAdAccessTokenResponse>(contentResult);
        }

    }
}
