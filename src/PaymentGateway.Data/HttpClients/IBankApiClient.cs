using PaymentGateway.Infrastructure.Models;

namespace PaymentGateway.Infrastructure.HttpClients
{
    public interface IBankApiClient
    {
        Task<BankApiPaymentResponse> SubmitPayment(BankApiPaymentRequest bankApiPayment);

        Task<BankApiPaymentResponse> GetPayment(long paymentId);
    }
}
