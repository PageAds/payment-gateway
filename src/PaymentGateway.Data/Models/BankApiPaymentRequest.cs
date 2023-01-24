namespace PaymentGateway.Data.Models
{
    public class BankApiPaymentRequest
    {
        public string CardNumber { get; set; }

        public DateTimeOffset CardExpiry { get; set; }

        public decimal Amount { get; set; }

        public string Currency { get; set; }

        public string CVV { get; set; }
    }
}
