using PaymentGateway.Domain.Models;
using PaymentGateway.API.Models;

namespace PaymentGateway.API.Mappers
{
    public class PaymentMapper : IPaymentMapper
    {
        public Payment Map(PaymentRequest paymentRequest)
        {
            return new Payment(paymentRequest.CardNumber, paymentRequest.CardExpiryMonth,
                paymentRequest.CardExpiryYear, paymentRequest.Amount, paymentRequest.Currency, paymentRequest.CVV);
        }
    }
}
