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

            RuleFor(x => x.CardExpiryMonth).GreaterThanOrEqualTo(1)
                .WithMessage("CardExpiryMonth must be between 1 and 12");

            RuleFor(x => x.CardExpiryMonth).LessThanOrEqualTo(12)
                .WithMessage("CardExpiryMonth must be between 1 and 12");

            RuleFor(x => x.CardExpiryYear).GreaterThanOrEqualTo(DateTimeOffset.UtcNow.Year)
                .WithMessage("CardExpiryYear must be assigned to the current year or future");
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
    }
}
