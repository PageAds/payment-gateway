namespace PaymentGateway.Models
{
    public class ValidationErrorModel
    {
        public string Message => "One or more validation errors occurred.";

        public IEnumerable<ErrorProperty> Properties { get; set; }
    }

    public class ErrorProperty
    {
        public string PropertyName { get; set; }

        public IEnumerable<string> Errors { get; set; }
    }
}
