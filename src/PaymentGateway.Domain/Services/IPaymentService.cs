using PaymentGateway.Domain.Models;

namespace PaymentGateway.Domain.Services
{
    public interface IPaymentService
    {
        Task<Payment> CreatePayment(Payment payment);
         
        Task<Payment> GetPayment(long id);
    }
}