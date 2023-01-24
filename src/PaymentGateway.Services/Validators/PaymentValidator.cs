using FluentValidation;
using PaymentGateway.Domain.Models;

namespace PaymentGateway.Services.Validators
{
    public class PaymentValidator : AbstractValidator<Payment>
    {
        public PaymentValidator()
        {
            RuleFor(x => x.CardNumber).NotEmpty()
                .WithMessage("Card number is mandatory");

            RuleFor(x => x.CardNumber).Length(12, 19) // according to https://www.validcreditcardnumber.com/
                .WithMessage("Card number must have a length between 12 and 19 numeric characters");

            RuleFor(x => x.CardNumber).Must(BeDigitsOnly)
                .WithMessage("Card number must only cotnain numeric characters");
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
