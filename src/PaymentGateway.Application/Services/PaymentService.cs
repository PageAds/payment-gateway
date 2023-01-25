using FluentValidation;
using PaymentGateway.Domain.Models;
using PaymentGateway.Domain.Services;
using PaymentGateway.Infrastructure.HttpClients;
using PaymentGateway.Infrastructure.Mappers;

namespace PaymentGateway.Application.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IValidator<Payment> paymentValidator;
        private readonly IBankApiClient bankApiClient;
        private readonly IBankApiPaymentRequestMapper requestMapper;
        private readonly IPaymentMapper paymentMapper;

        public PaymentService(
            IValidator<Payment> paymentValidator,
            IBankApiClient bankApiClient,
            IBankApiPaymentRequestMapper requestMapper,
            IPaymentMapper paymentMapper)
        {
            this.paymentValidator = paymentValidator;
            this.bankApiClient = bankApiClient;
            this.requestMapper = requestMapper;
            this.paymentMapper = paymentMapper;
        }

        public async Task<Payment> CreatePayment(Payment payment)
        {
            paymentValidator.ValidateAndThrow(payment);

            var bankApiPaymentResponse = await bankApiClient.SubmitPayment(requestMapper.Map(payment));

            return paymentMapper.Map(bankApiPaymentResponse);
        }

        public async Task<Payment> GetPayment(long id)
        {
            var bankApiPaymentResponse = await bankApiClient.GetPayment(id);

            if (bankApiPaymentResponse == null)
                return null;

            return paymentMapper.Map(bankApiPaymentResponse);
        }
    }
}
