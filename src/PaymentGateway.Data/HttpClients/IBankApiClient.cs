using PaymentGateway.Data.Models;

namespace PaymentGateway.Data.HttpClients
{
    public interface IBankApiClient
    {
        Task<BankApiPaymentResponse> SubmitPayment(BankApiPaymentRequest bankApiPayment);

        Task<BankApiPaymentResponse> GetPayment(long paymentId);
    }
}
