namespace PaymentGateway.API.Mappers
{
    public interface IPaymentViewModelMapper
    {
        Models.ViewModels.Payment Map(Domain.Models.Payment payment);
    }
}
