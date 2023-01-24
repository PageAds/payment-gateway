namespace PaymentGateway.Data.Helpers
{
    public class IdGenerator : IIdGenerator
    {
        private int counter = 0;

        public long Generate() => counter +=1;
    }
}
