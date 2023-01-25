using PaymentGateway.Domain.Models;
using PaymentGateway.Infrastructure.Models;

namespace PaymentGateway.Infrastructure.Mappers
{
    public interface IPaymentMapper
    {
        Payment Map(BankApiPaymentResponse response);
    }
}
