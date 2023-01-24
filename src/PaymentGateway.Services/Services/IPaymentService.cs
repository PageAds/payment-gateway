using PaymentGateway.Domain.Models;

namespace PaymentGateway.Services.Services
{
    public interface IPaymentService
    {
        Payment CreatePayment(Payment payment);
    }
}