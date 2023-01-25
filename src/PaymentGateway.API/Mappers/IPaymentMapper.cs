using PaymentGateway.Domain.Models;
using PaymentGateway.Models;

namespace PaymentGateway.Mappers
{
    public interface IPaymentMapper
    {
        Payment Map(PaymentRequest paymentRequest);
    }
}