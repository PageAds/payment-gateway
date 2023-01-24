using FluentValidation;
using PaymentGateway.Domain.Models;

namespace PaymentGateway.Services.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IValidator<Payment> paymentValidator;

        public PaymentService(IValidator<Payment> paymentValidator)
        {
            this.paymentValidator = paymentValidator;
        }

        public Payment CreatePayment(Payment payment)
        {
            paymentValidator.ValidateAndThrow(payment);

            payment.Id = 1;

            return payment;
        }
    }
}
