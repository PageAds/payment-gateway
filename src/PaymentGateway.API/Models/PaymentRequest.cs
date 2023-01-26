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

        /// <summary>
        /// Primary account number
        /// </summary>
        public string CardNumber { get; }

        /// <summary>
        /// The month the card expires in numerical format (1 through 12)
        /// </summary>
        public int CardExpiryMonth { get; }

        /// <summary>
        /// The year the card expires (yyyy)
        /// </summary>
        public int CardExpiryYear { get; }

        /// <summary>
        /// Payment amount
        /// </summary>
        public decimal Amount { get; }

        /// <summary>
        /// Currency of the payment in ISO 4217 format (e.g. "GBP" or "USD")
        /// </summary>
        public string Currency { get; }

        /// <summary>
        /// Card verification value
        /// </summary>
        public string CVV { get; }
    }
}
