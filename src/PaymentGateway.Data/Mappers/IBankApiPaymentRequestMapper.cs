using PaymentGateway.Data.Models;
using PaymentGateway.Domain.Models;

namespace PaymentGateway.Data.Mappers
{
    public interface IBankApiPaymentRequestMapper
    {
        BankApiPaymentRequest Map(Payment payment);
    }
}