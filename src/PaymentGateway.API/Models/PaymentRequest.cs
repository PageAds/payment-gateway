namespace PaymentGateway.API.Models
{
    public class PaymentRequest
    {
        public PaymentRequest(string cardNumber, int cardExpiryMonth, int cardExpiryYear, decimal amount, string currency, string cvv)
        {
            CardNumber = cardNumber;
            CardExpiryMonth = cardExpiryMonth;
            CardExpiryYear = cardExpiryYear;
            Amount = amount;
            Currency = currency;
            CVV = cvv;
        }

        public string CardNumber { get; }

        public int CardExpiryMonth { get; }

        public int CardExpiryYear { get; }

        public decimal Amount { get; }

        public string Currency { get; }

        public string CVV { get; }
    }
}
