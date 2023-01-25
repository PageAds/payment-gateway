using PaymentGateway.Domain.Models;
using PaymentGateway.Infrastructure.Models;

namespace PaymentGateway.Infrastructure.Mappers
{
    public interface IBankApiPaymentRequestMapper
    {
        BankApiPaymentRequest Map(Payment payment);
    }
}