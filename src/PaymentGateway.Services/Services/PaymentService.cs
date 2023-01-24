using FluentValidation;
using PaymentGateway.Data.HttpClients;
using PaymentGateway.Data.Mappers;
using PaymentGateway.Domain.Models;
using PaymentGateway.Domain.Repositories;

namespace PaymentGateway.Services.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IValidator<Payment> paymentValidator;
        private readonly IPaymentRepository paymentRepository;
        private readonly IBankApiClient bankApiClient;
        private readonly IBankApiPaymentRequestMapper requestMapper;

        public PaymentService(
            IValidator<Payment> paymentValidator,
            IPaymentRepository paymentRepository,
            IBankApiClient bankApiClient,
            IBankApiPaymentRequestMapper requestMapper)
        {
            this.paymentValidator = paymentValidator;
            this.paymentRepository = paymentRepository;
            this.bankApiClient = bankApiClient;
            this.requestMapper = requestMapper;
        }

        public async Task<Payment> CreatePayment(Payment payment)
        {
            paymentValidator.ValidateAndThrow(payment);

            // Submit the payment to the bank
            var bankApiPaymentResponse = await bankApiClient.SubmitPayment(requestMapper.Map(payment));

            // Assign properties returned from bank api
            payment.ExternalId = bankApiPaymentResponse.Id;
            payment.Status = bankApiPaymentResponse.Status;

            // Save payment to our data store
            payment = await paymentRepository.Save(payment);

            return payment;
        }

        public async Task<Payment> GetPayment(long id)
        {
            // Get payment from our data store so we can resolve the payment external id (id of the payment on the banks side)
            var payment = await paymentRepository.Get(id);

            if (payment == null)
                return null;

            var bankApiPaymentResponse = await bankApiClient.GetPayment(payment.ExternalId);

            if (payment.Status != bankApiPaymentResponse.Status)
            {
                // If status has changed, save payment to our data store
                payment = await paymentRepository.Save(payment);
            }

            return payment;
        }
    }
}
