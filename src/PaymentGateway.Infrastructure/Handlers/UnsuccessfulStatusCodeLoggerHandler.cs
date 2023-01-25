using Microsoft.Extensions.Logging;

namespace PaymentGateway.Infrastructure.Handlers
{
    public class UnsuccessfulStatusCodeLoggerHandler : DelegatingHandler
    {
        private readonly ILogger<UnsuccessfulStatusCodeLoggerHandler> logger;

        public UnsuccessfulStatusCodeLoggerHandler(ILogger<UnsuccessfulStatusCodeLoggerHandler> logger)
        {
            this.logger = logger;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var httpResponseMessage = await base.SendAsync(request, cancellationToken);

            var responseString = await httpResponseMessage.Content.ReadAsStringAsync();

            if (!httpResponseMessage.IsSuccessStatusCode)
            {
                logger.LogError($"Request: {httpResponseMessage?.RequestMessage?.Method} {httpResponseMessage?.RequestMessage?.RequestUri} " +
                    $"failed with status code: {httpResponseMessage?.StatusCode} with response content: {responseString}");
            }

            return httpResponseMessage;
        }
    }
}
