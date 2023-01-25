namespace PaymentGateway.API.Models
{
    public class ErrorResponse
    {
        public List<Error> Errors { get; set; } = new List<Error>();
    }

    public class Error
    {
        public string FieldName { get; set; }

        public string Message { get; set; }
    }
}
