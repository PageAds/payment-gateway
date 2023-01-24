using Microsoft.Extensions.Caching.Memory;
using PaymentGateway.Data.Helpers;
using PaymentGateway.Domain.Models;
using PaymentGateway.Domain.Repositories;

namespace PaymentGateway.Data.Repositories
{
    public class InMemoryPaymentRepository : IPaymentRepository
    {
        private readonly IMemoryCache memoryCache;
        private readonly IIdGenerator idGenerator;

        private const string CacheKeyPrefix = "Payment-";

        public InMemoryPaymentRepository(
            IMemoryCache memoryCache,
            IIdGenerator idGenerator)
        {
            this.memoryCache = memoryCache;
            this.idGenerator = idGenerator;
        }

        public async Task<Payment> Get(long id)
        {
            var cacheKey = $"{CacheKeyPrefix}{id}";
            return memoryCache.Get<Payment>(cacheKey);
        }

        public async Task<Payment> Save(Payment payment)
        {
            payment.Id = idGenerator.Generate();
            var cacheKey = $"{CacheKeyPrefix}{payment.Id}";
            
            return memoryCache.Set(cacheKey, payment);
        }
    }
}
