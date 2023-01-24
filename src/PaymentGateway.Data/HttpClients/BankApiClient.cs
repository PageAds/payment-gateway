using PaymentGateway.Data.Models;

namespace PaymentGateway.Data.HttpClients
{
    public class BankApiClient : IBankApiClient
    {
        public async Task<BankApiPaymentResponse> GetPayment(long paymentId)
        {
            return new BankApiPaymentResponse
            {
                Id = 1,
                Status = "Pending",
            };
        }

        public async Task<BankApiPaymentResponse> SubmitPayment(BankApiPaymentRequest bankApiPayment)
        {
            return new BankApiPaymentResponse
            {
                Id = 1,
                Status = "Pending",
            };
        }
    }
}
