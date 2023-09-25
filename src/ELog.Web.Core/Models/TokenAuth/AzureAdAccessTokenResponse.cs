using Newtonsoft.Json;

namespace ELog.Web.Core.Models.TokenAuth
{
    public class AzureAdAccessTokenResponse
    {
        [JsonProperty("token_type")]
        public string TokenType
        {
            get; set;
        }


        [JsonProperty("access_token")]
        public string AccessToken
        {
            get; set;
        }
    }
}
