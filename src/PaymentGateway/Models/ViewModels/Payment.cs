namespace PaymentGateway.Models.ViewModels
{
    public class Payment
    {
        public Payment(long id, string status, string cardNumber, decimal amount, string currency)
        {
            Id = id;
            Status = status;

            // Mask card number except last 4 digits
            CardNumber = cardNumber.Substring(cardNumber.Length - 4).PadLeft(cardNumber.Length, '*');

            Amount = amount;
            Currency = currency;
        }

        public long Id { get; }

        public string Status { get; }

        public string CardNumber { get; }

        public decimal Amount { get; }

        public string Currency { get; }
    }
}
