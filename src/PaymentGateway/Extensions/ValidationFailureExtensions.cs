using FluentValidation.Results;
using PaymentGateway.Models;

namespace PaymentGateway.Extensions
{
    public static class ValidationFailureExtensions
    {
        public static IEnumerable<ErrorProperty> ToErrorProperties(this IEnumerable<ValidationFailure> validationFailures)
        {
            return validationFailures
                .GroupBy(x => x.PropertyName)
                .Select(group => new ErrorProperty 
                { 
                    PropertyName = group.Key, 
                    Errors = group.ToList().Select(x => x.ErrorMessage) 
                });
        }
    }
}
