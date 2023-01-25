namespace PaymentGateway.API.Models.ViewModels
{
    public class Payment
    {
        public Payment(long id, string status, string cardNumber, int cardExpiryMonth, int cardExpiryYear, decimal amount, string currency, string cvv)
        {
            Id = id;
            Status = status;

            // Mask card number except last 4 digits
            CardNumber = cardNumber.Substring(cardNumber.Length - 4).PadLeft(cardNumber.Length, '*');

            CardExpiryMonth = cardExpiryMonth;
            CardExpiryYear = cardExpiryYear;
            CVV = cvv;
            Amount = amount;
            Currency = currency;
        }

        public long Id { get; }

        public string Status { get; }

        public string CardNumber { get; }

        public int CardExpiryMonth { get; }

        public int CardExpiryYear { get; }

        public decimal Amount { get; }

        public string Currency { get; }

        public string CVV { get; }
    }
}
