using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace moondraft.Logging
{
    public class HttpClientLoggingHandler : DelegatingHandler
    {
        public HttpClientLoggingHandler(HttpMessageHandler innerHandler)
            : base(innerHandler)
        {
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            System.Diagnostics.Debug.WriteLine("Before download: " + request.RequestUri);
            HttpResponseMessage response = await base.SendAsync(request, cancellationToken);
            System.Diagnostics.Debug.WriteLine("After download: " + request.RequestUri);
            return response;
        }
    }
}
