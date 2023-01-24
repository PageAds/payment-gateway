using FluentValidation;
using PaymentGateway.Domain.Models;

namespace PaymentGateway.Services.Validators
{
    public class PaymentValidator : AbstractValidator<Payment>
    {
        private IEnumerable<string> validCurrencies = new List<string> { "GBP", "USD" };

        public PaymentValidator()
        {
            RuleFor(x => x.CardNumber).NotEmpty()
                .WithMessage("CardNumber is mandatory");

            RuleFor(x => x.CardNumber).Length(12, 19) // according to https://www.validcreditcardnumber.com/
                .WithMessage("CardNumber must have a length between 12 and 19 numeric characters");

            RuleFor(x => x.CardNumber).Must(BeDigitsOnly)
                .WithMessage("CardNumber must only contain numeric characters");

            RuleFor(x => x.CardExpiryMonth).Must(BeAValidExpiryMonth)
                .WithMessage("CardExpiryMonth must be between 1 and 12");

            RuleFor(x => x.CardExpiryYear).GreaterThanOrEqualTo(DateTimeOffset.UtcNow.Year)
                .WithMessage("CardExpiryYear is in the past");

            RuleFor(x => x).Must(BeACardExpiryInTheFuture)
                .WithName("CardExpiryMonth")
                .WithMessage("CardExpiryMonth is in the past")
                .When(x => BeAValidExpiryMonth(x.CardExpiryMonth));
           
            RuleFor(x => x.Amount).GreaterThan(0)
                .WithMessage("Amount must be greater than 0");

            RuleFor(x => x.Currency).NotEmpty()
                .WithMessage("Currency is mandatory");

            RuleFor(x => x.Currency).Must(BeAValidCurrency)
                .WithMessage($"Currency is invalid. Must be one of the following supported currencies: {string.Join(", ", validCurrencies)}");

            RuleFor(x => x.CVV).NotEmpty()
                .WithMessage("CVV is mandatory");

            RuleFor(x => x.CVV).Length(3)
                .WithMessage("CVV must be 3 numeric characters");
            
            RuleFor(x => x.CVV).Must(BeDigitsOnly)
                .WithMessage("CVV must only contain numeric characters");
        }

        private bool BeDigitsOnly(string str)
        {
            foreach (char c in str)
            {
                if (c < '0' || c > '9')
                    return false;
            }

            return true;
        }

        private bool BeAValidExpiryMonth(int cardExpiryMonth)
        {
            return cardExpiryMonth >= 1 && cardExpiryMonth <= 12;
        }

        private bool BeACardExpiryInTheFuture(Payment payment)
        {
            var lastDayOfMonth = DateTime.DaysInMonth(payment.CardExpiryYear, payment.CardExpiryMonth);
            var cardExiryDateTime = new DateTimeOffset(payment.CardExpiryYear, payment.CardExpiryMonth, lastDayOfMonth, 0, 0, 0, default);

            return cardExiryDateTime >= DateTimeOffset.UtcNow;
        }

        private bool BeAValidCurrency(string currency)
        {
            return validCurrencies.Contains(currency);
        }
    }
}
