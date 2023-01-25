using PaymentGateway.API.Models;
using PaymentGateway.Domain.Models;

namespace PaymentGateway.API.Mappers
{
    public interface IPaymentMapper
    {
        Payment Map(PaymentRequest paymentRequest);
    }
}