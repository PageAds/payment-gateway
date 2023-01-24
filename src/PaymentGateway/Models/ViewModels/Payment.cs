namespace PaymentGateway.Models.ViewModels
{
    public class Payment
    {
        public Payment(long id, string status, string cardNumber, decimal amount, string currency)
        {
            Id = id;
            Status = status;
            CardNumber = cardNumber;
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
