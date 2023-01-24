using PaymentGateway.Domain.Models;

namespace PaymentGateway.Domain.Repositories
{
    public interface IPaymentRepository
    {
        Task<Payment> Get(long id);

        Task<Payment> Save(Payment payment);
    }
}
