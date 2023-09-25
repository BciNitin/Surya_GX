using Polly;

using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SigmaCrew.APIClient
{
    public class HttpRetryMessageHandler : DelegatingHandler
    {
        public HttpRetryMessageHandler(HttpClientHandler handler) : base(handler)
        {
        }

        protected override Task<HttpResponseMessage> SendAsync(
          HttpRequestMessage request,
          CancellationToken cancellationToken) =>
          Policy
              .Handle<HttpRequestException>()
              .Or<TaskCanceledException>()
              .OrResult<HttpResponseMessage>(x => !x.IsSuccessStatusCode)
              .WaitAndRetryAsync(5, retryAttempt => TimeSpan.FromSeconds(Math.Pow(1, retryAttempt)), async (result, timeSpan, retryCount, context) =>
              {
                  if (retryCount == 5)
                  {
                      throw result.Exception;
                  }
              }).ExecuteAsync(() => base.SendAsync(request, cancellationToken));
    }
}