using PaymentGateway.Infrastructure.HttpClients;
using PaymentGateway.Infrastructure.Models;

namespace PaymentGateway.AcceptanceTests.Mocks
{
    public class BankApiClientMock : IBankApiClient
    {
        private BankApiPaymentResponse submittedPaymentResponse;

        public async Task<BankApiPaymentResponse> GetPayment(long paymentId)
        {
            return submittedPaymentResponse;
        }

        public async Task<BankApiPaymentResponse> SubmitPayment(BankApiPaymentRequest bankApiPayment)
        {
            submittedPaymentResponse = new BankApiPaymentResponse
            {
                Id = 1,
                Status = "Pending",
                Amount = bankApiPayment.Amount,
                CardExpiry = bankApiPayment.CardExpiry,
                CardNumber = bankApiPayment.CardNumber,
                Currency = bankApiPayment.Currency,
                CVV = bankApiPayment.CVV
            };

            return submittedPaymentResponse;
        }
    }
}
