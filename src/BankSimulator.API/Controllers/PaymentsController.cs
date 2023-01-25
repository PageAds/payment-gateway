using BankSimulator.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using PaymentGateway.Infrastructure.Models;

namespace BankSimulator.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PaymentsController : ControllerBase
    {
        private readonly IMemoryCache memoryCache;
        private readonly IIdGenerator idGenerator;
        private const string CacheKeyPrefix = "Payment-";

        public PaymentsController(
            IMemoryCache memoryCache,
            IIdGenerator idGenerator)
        {
            this.memoryCache = memoryCache;
            this.idGenerator = idGenerator;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(BankApiPaymentResponse))]
        public IActionResult Submit(BankApiPaymentRequest paymentRequest)
        {
            var paymentResponse = new BankApiPaymentResponse();

            paymentResponse.Amount = paymentRequest.Amount;
            paymentResponse.CardExpiry = paymentRequest.CardExpiry;
            paymentResponse.CardNumber = paymentRequest.CardNumber;
            paymentResponse.Currency = paymentRequest.Currency;
            paymentResponse.CVV = paymentRequest.CVV;

            paymentResponse.Status = "Pending";
            paymentResponse.Id = idGenerator.Generate();
            var cacheKey = $"{CacheKeyPrefix}{paymentResponse.Id}";

            memoryCache.Set(cacheKey, paymentResponse);

            return CreatedAtAction(nameof(Submit), paymentResponse);
        }

        [HttpGet("{paymentId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BankApiPaymentResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(long paymentId)
        {
            var cacheKey = $"{CacheKeyPrefix}{paymentId}";
            var payment = memoryCache.Get<BankApiPaymentResponse>(cacheKey);

            if (payment == null)
            {
                return NotFound();
            }

            payment.Status = "Completed";
            memoryCache.Set(cacheKey, payment);

            return Ok(payment);
        }
    }
}