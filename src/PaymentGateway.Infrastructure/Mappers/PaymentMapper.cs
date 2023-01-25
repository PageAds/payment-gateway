using PaymentGateway.Domain.Models;
using PaymentGateway.Infrastructure.Models;

namespace PaymentGateway.Infrastructure.Mappers
{
    public class PaymentMapper : IPaymentMapper
    {
        public Payment Map(BankApiPaymentResponse response)
        {
            return new Payment(response.Id, response.Status, response.CardNumber, response.CardExpiry.Month,
                response.CardExpiry.Year, response.Amount, response.Currency, response.CVV);
        }
    }
}
