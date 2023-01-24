namespace PaymentGateway.Mappers
{
    public class PaymentViewModelMapper : IPaymentViewModelMapper
    {
        public Models.ViewModels.Payment Map(Domain.Models.Payment payment)
        {
            return new Models.ViewModels.Payment(payment.Id, payment.Status, payment.CardNumber, payment.Amount, payment.Currency);
        }
    }
}
