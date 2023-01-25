using FluentValidation.Results;
using PaymentGateway.Models;

namespace PaymentGateway.Extensions
{
    public static class ValidationFailureExtensions
    {
        public static IEnumerable<Error> ToErrorProperties(this IEnumerable<ValidationFailure> validationFailures)
        {
            return validationFailures
                .Select(x => new Error 
                { 
                    FieldName = x.PropertyName,
                    Message = x.ErrorMessage
                });
        }
    }
}
