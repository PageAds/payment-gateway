namespace PaymentGateway.API.Mappers
{
    public class PaymentViewModelMapper : IPaymentViewModelMapper
    {
        public Models.ViewModels.Payment Map(Domain.Models.Payment payment)
        {
            return new Models.ViewModels.Payment(payment.Id, payment.Status, payment.CardNumber, payment.CardExpiryMonth, 
                payment.CardExpiryYear, payment.Amount, payment.Currency, payment.CVV);
        }
    }
}
