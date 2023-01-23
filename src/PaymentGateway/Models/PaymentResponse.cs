namespace PaymentGateway.Models
{
    public class PaymentResponse : PaymentRequest
    {
        public PaymentResponse(string cardNumber, int cardExpiryMonth, int cardExpiryYear, decimal amount, string currency, string cvv, long id)
            : base(cardNumber, cardExpiryMonth, cardExpiryYear, amount, currency, cvv)
        {
            Id = id;
        }

        public long Id { get; }
    }
}
