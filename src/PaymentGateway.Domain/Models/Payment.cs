namespace PaymentGateway.Domain.Models
{
    public class Payment
    {

        /// <summary>
        /// Used for when we do not have a known id or status
        /// </summary>
        /// <param name="cardNumber"></param>
        /// <param name="cardExpiryMonth"></param>
        /// <param name="cardExpiryYear"></param>
        /// <param name="amount"></param>
        /// <param name="currency"></param>
        /// <param name="cvv"></param>
        public Payment(string cardNumber, int cardExpiryMonth, int cardExpiryYear, decimal amount, string currency, string cvv)
        {
            CardNumber = cardNumber;
            CardExpiryMonth = cardExpiryMonth;
            CardExpiryYear = cardExpiryYear;
            Amount = amount;
            Currency = currency;
            CVV = cvv;
        }

        /// <summary>
        /// Used for when id and status is known
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        /// <param name="cardNumber"></param>
        /// <param name="cardExpiryMonth"></param>
        /// <param name="cardExpiryYear"></param>
        /// <param name="amount"></param>
        /// <param name="currency"></param>
        /// <param name="cvv"></param>
        public Payment(long id, string status, string cardNumber, int cardExpiryMonth, int cardExpiryYear, decimal amount, string currency, string cvv)
        {
            Id = id;
            Status = status;
            CardNumber = cardNumber;
            CardExpiryMonth = cardExpiryMonth;
            CardExpiryYear = cardExpiryYear;
            Amount = amount;
            Currency = currency;
            CVV = cvv;
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
