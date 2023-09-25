using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.PowerBI.Api;
using Microsoft.PowerBI.Api.Models;
using Microsoft.Rest;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ELog.Web.Host.Startup
{
    public static class AuthConfigurer
    {
        private static SerializationInfo AuthorityUrl;

        public static string ResourceUrl { get; private set; }
        public static string ApplicationSecret { get; private set; }
        public static string MasterUsername { get; private set; }
        public static string MasterPassword { get; private set; }
        public static Guid WorkspaceId { get; private set; }
        public static Guid ReportId { get; private set; }
        public static string ClientId { get; private set; }

        public static async Task ConfigureAsync(IServiceCollection services, IConfiguration configuration)
        {
            //var Username = "pharmision@barcodeindia.onmicrosoft.com";
            //var Password = "Bcil@1234";
            //var AuthorityUrl = "https://app.powerbi.com/reportEmbed?reportId=a504fece-eaf1-4090-a67b-f291e546f05c&autoAuth=true&ctid=483ae985-aac7-4952-81a3-687c278d43b6&config=eyJjbHVzdGVyVXJsIjoiaHR0cHM6Ly93YWJpLWluZGlhLXdlc3QtcmVkaXJlY3QuYW5hbHlzaXMud2luZG93cy5uZXQvIn0%3D";

            //var authenticationResult = await AuthenticateAsync();
            //var credential = new UserPasswordCredential(Username, Password);

            //// Authenticate using created credentials
            //var authenticationContext = new AuthenticationContext(AuthorityUrl);
            //var authenticationResult = await authenticationContext.AcquireTokenAsync((ResourceUrl, ClientId, credential);


            if (bool.Parse(configuration["Authentication:JwtBearer:IsEnabled"]))
            {
                services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = "JwtBearer";
                    options.DefaultChallengeScheme = "JwtBearer";
                }).AddJwtBearer("JwtBearer", options =>
                {
                    options.Audience = configuration["Authentication:JwtBearer:Audience"];

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        // The signing key must match!
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration["Authentication:JwtBearer:SecurityKey"])),

                        // Validate the JWT Issuer (iss) claim
                        ValidateIssuer = true,
                        ValidIssuer = configuration["Authentication:JwtBearer:Issuer"],

                        // Validate the JWT Audience (aud) claim
                        ValidateAudience = true,
                        ValidAudience = configuration["Authentication:JwtBearer:Audience"],

                        // Validate the token expiry
                        ValidateLifetime = true,

                        // If you want to allow a certain amount of clock drift, set that here
                        ClockSkew = TimeSpan.Zero
                    };

                });
            }
            EmbedReport();
        }

        //private static async Task<OAuthResult> AuthenticateAsync()
        //{
        //    var oauthEndpoint = new Uri(AuthorityUrl);

        //    using (var client = new HttpClient())
        //    {
        //        var result = await client.PostAsync(oauthEndpoint, new FormUrlEncodedContent(new[]
        //        {
        //    new KeyValuePair<string, string>("resource", ResourceUrl),
        //    new KeyValuePair<string, string>("client_id", ClientId),
        //    new KeyValuePair<string, string>("grant_type", "password"),
        //    new KeyValuePair<string, string>("username", Username),
        //    new KeyValuePair<string, string>("password", Password),
        //    new KeyValuePair<string, string>("scope", "openid"),
        //}));

        //        var content = await result.Content.ReadAsStringAsync();
        //        return JsonConvert.DeserializeObject<OAuthResult>(content);
        //    }
        //}

        [HttpGet("EmbedConfig")]
        public static EmbedConfig EmbedReport()
        {
            EmbedConfig report = null;
            var proUserToken = AuthenticateUATAsync().GetAwaiter().GetResult();
            report = GenerateReport((string)proUserToken.AccessToken);
            return report;
        }

        /*  private static EmbedConfig GenerateReport(object accessToken)
          {
              throw new NotImplementedException();
          }*/

        private static async Task<OAuthResult> AuthenticateUATAsync()
        {
            var AuthorityUrl = "https://app.powerbi.com/reportEmbed?reportId=a504fece-eaf1-4090-a67b-f291e546f05c&autoAuth=true&ctid=483ae985-aac7-4952-81a3-687c278d43b6&config=eyJjbHVzdGVyVXJsIjoiaHR0cHM6Ly93YWJpLWluZGlhLXdlc3QtcmVkaXJlY3QuYW5hbHlzaXMud2luZG93cy5uZXQvIn0%3D";
            var oauthEndpoint = new Uri(AuthorityUrl);

            using (var client = new HttpClient())
            {
                var result = await client.PostAsync(oauthEndpoint, new FormUrlEncodedContent(new[] {
            new KeyValuePair<string,string>("resource", ResourceUrl),
             new KeyValuePair<string, string>("client_id", ClientId),
            //new KeyValuePair<string,string>("client_id", ApplicationId),
            new KeyValuePair<string,string>("client_secret", ApplicationSecret),
            new KeyValuePair<string,string>("grant_type", "password"),
            new KeyValuePair<string,string>("username", "pharmision@barcodeindia.onmicrosoft.com"),
            new KeyValuePair<string,string>("password", "Bcil@1234"),
            new KeyValuePair<string,string>("scope", "openid")
        }));

                var content = await result.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<OAuthResult>(content);
            }
        }

        private static EmbedConfig GenerateReport(string token, List<EffectiveIdentity> filters = null)
        {
            EmbedConfig config = null;
            var tokenCredentials = new TokenCredentials(token, "Bearer");

            using (var client = new PowerBIClient(new Uri("https://api.powerbi.com"), tokenCredentials))
            {
                Report report = client.Reports.GetReportInGroup(WorkspaceId, ReportId);
                if (report != null)
                {
                    var requestParameters = new GenerateTokenRequest();
                    requestParameters.AccessLevel = "View";
                    if (filters != null)
                    {
                        requestParameters.Identities = filters;
                    }

                    EmbedToken embedtoken = client.Reports.GenerateTokenInGroupAsync(WorkspaceId, ReportId, requestParameters).GetAwaiter().GetResult();

                    config = new EmbedConfig();
                    config.EmbedURL = report.EmbedUrl;
                    config.GroupID = WorkspaceId;
                    config.ReportData = report;
                    config.ReportID = ReportId;
                    config.Token = embedtoken?.Token;
                    config.TokenID = embedtoken?.TokenId;
                    config.Expiration = embedtoken?.Expiration;
                }
            }
            return config;
        }



    }
}
