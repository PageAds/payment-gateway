using FluentValidation.Results;
using PaymentGateway.API.Models;

namespace PaymentGateway.API.Extensions
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
