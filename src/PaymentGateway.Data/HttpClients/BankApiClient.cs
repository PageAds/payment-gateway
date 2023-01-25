using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PaymentGateway.Infrastructure.Models;
using System.Net;
using System.Net.Http.Json;

namespace PaymentGateway.Infrastructure.HttpClients
{
    public class BankApiClient : IBankApiClient
    {
        private readonly HttpClient httpClient;
        private readonly ILogger<BankApiClient> logger;

        public BankApiClient(
            HttpClient httpClient,
            ILogger<BankApiClient> logger)
        {
            this.httpClient = httpClient;
            this.logger = logger;
        }

        public async Task<BankApiPaymentResponse> GetPayment(long paymentId)
        {
            var httpResponseMessage = await httpClient.GetAsync(@$"payments/{paymentId}");

            if (httpResponseMessage.StatusCode == HttpStatusCode.NotFound)
            {
                logger.LogError($"Could not find payment: {paymentId} using GET {httpResponseMessage?.RequestMessage?.RequestUri}");
                return null;
            }

            var responseString = await httpResponseMessage.Content.ReadAsStringAsync();

            if (!httpResponseMessage.IsSuccessStatusCode)
            {
                logger.LogError($"Failed to get payment using GET {httpResponseMessage?.RequestMessage?.RequestUri} " +
                    $"with status code: {httpResponseMessage.StatusCode} and response body: {responseString}");
                throw new Exception("Failed to get payment");
            }

            return JsonConvert.DeserializeObject<BankApiPaymentResponse>(responseString);
        }

        public async Task<BankApiPaymentResponse> SubmitPayment(BankApiPaymentRequest bankApiPayment)
        {
            var httpResponseMessage = await httpClient.PostAsJsonAsync(@$"payments", bankApiPayment);

            var responseString = await httpResponseMessage.Content.ReadAsStringAsync();

            if (!httpResponseMessage.IsSuccessStatusCode)
            {
                logger.LogError($"Failed to submit payment using POST {httpResponseMessage?.RequestMessage?.RequestUri} " +
                    $"with status code: {httpResponseMessage.StatusCode} and response body: {responseString}");
                throw new Exception("Failed to submit payment");
            }

            return JsonConvert.DeserializeObject<BankApiPaymentResponse>(responseString);
        }
    }
}
