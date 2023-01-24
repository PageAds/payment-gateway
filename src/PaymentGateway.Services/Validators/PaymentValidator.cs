using FluentValidation;
using PaymentGateway.Domain.Models;

namespace PaymentGateway.Services.Validators
{
    public class PaymentValidator : AbstractValidator<Payment>
    {
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
                .WithName("CardExpiry")
                .WithMessage("CardExpiry is in the past")
                .When(x => BeAValidExpiryMonth(x.CardExpiryMonth));
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
    }
}
