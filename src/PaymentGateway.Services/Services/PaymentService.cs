using FluentValidation;
using PaymentGateway.Domain.Models;
using PaymentGateway.Domain.Repositories;

namespace PaymentGateway.Services.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IValidator<Payment> paymentValidator;
        private readonly IPaymentRepository paymentRepository;

        public PaymentService(
            IValidator<Payment> paymentValidator,
            IPaymentRepository paymentRepository)
        {
            this.paymentValidator = paymentValidator;
            this.paymentRepository = paymentRepository;
        }

        public async Task<Payment> CreatePayment(Payment payment)
        {
            paymentValidator.ValidateAndThrow(payment);

            await paymentRepository.Save(payment);

            return payment;
        }

        public async Task<Payment> GetPayment(long id)
        {
            return await paymentRepository.Get(id);
        }
    }
}
