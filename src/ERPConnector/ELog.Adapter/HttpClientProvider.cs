using Microsoft.Extensions.Configuration;

using SigmaCrew.APIClient;

using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace ELog.Adapter
{
    public class HttpClientProvider : IHttpClientProvider
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public HttpClientProvider(IConfiguration configuration)
        {
            _configuration = configuration;

            _httpClient = new HttpClient(new HttpRetryMessageHandler(new HttpClientHandler()))
            {
                Timeout = TimeSpan.FromSeconds(45)
            };

            _httpClient.DefaultRequestHeaders.Add("ContentType", "application/json");

            var sAPApiCredentials = _configuration["SAPApiCredentials:UsernamePassword"];
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(sAPApiCredentials);
            string val = System.Convert.ToBase64String(plainTextBytes);
            _httpClient.DefaultRequestHeaders.Add("Authorization", "Basic " + val);
        }

        public Task<HttpResponseMessage> GetAsync(string requestUri)
        {
            return _httpClient.GetAsync(requestUri);
        }

        public Task<HttpResponseMessage> PostAsync(string requestUri, HttpContent content)
        {
            return _httpClient.PostAsync(requestUri, content);
        }

        public Task<HttpResponseMessage> PutAsync(string requestUri, HttpContent content)
        {
            return _httpClient.PutAsync(requestUri, content);
        }

        public Task<HttpResponseMessage> DeleteAsync(string requestUri)
        {
            return _httpClient.DeleteAsync(requestUri);
        }
    }
}