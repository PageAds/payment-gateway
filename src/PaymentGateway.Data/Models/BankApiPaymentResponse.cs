namespace PaymentGateway.Infrastructure.Models
{
    public class BankApiPaymentResponse
    {
        public long Id { get; set; }

        public string Status { get; set; }

        public string CardNumber { get; set; }

        public DateTimeOffset CardExpiry { get; set; }

        public decimal Amount { get; set; }

        public string Currency { get; set; }

        public string CVV { get; set; }
    }
}
