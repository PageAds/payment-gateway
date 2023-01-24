using PaymentGateway.Domain.Models;

namespace PaymentGateway.Services.Services
{
    public interface IPaymentService
    {
        Task<Payment> CreatePayment(Payment payment);

        Task<Payment> GetPayment(long id);
    }
}