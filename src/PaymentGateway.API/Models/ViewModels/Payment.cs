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

        /// <summary>
        /// Identifier of the payment
        /// </summary>
        public long Id { get; }

        /// <summary>
        /// Status of the payment (either "Pending" or "Completed")
        /// </summary>
        public string Status { get; }

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
        /// Currency of the payment in ISO 4217 format (e.g. "GBP")
        /// </summary>
        public string Currency { get; }

        /// <summary>
        /// Card verification value
        /// </summary>
        public string CVV { get; }
    }
}
