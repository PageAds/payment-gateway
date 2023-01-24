using PaymentGateway.Data.Models;
using PaymentGateway.Domain.Models;

namespace PaymentGateway.Data.Mappers
{
    public class BankApiPaymentRequestMapper : IBankApiPaymentRequestMapper
    {
        public BankApiPaymentRequest Map(Payment payment)
        {
            return new BankApiPaymentRequest
            {
                Amount = payment.Amount,
                CardExpiry = new DateTimeOffset(payment.CardExpiryYear, payment.CardExpiryMonth, DateTime.DaysInMonth(payment.CardExpiryYear, payment.CardExpiryMonth), 0, 0, 0, default),
                CardNumber = payment.CardNumber,
                Currency = payment.Currency,
                CVV = payment.CVV
            };
        }
    }
}
