using Microsoft.AspNetCore.Mvc;
using PaymentGateway.Models;

namespace PaymentGateway.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PaymentsController : ControllerBase
    {
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> Create(PaymentRequest paymentRequest)
        {
            var paymentResponse = new PaymentResponse(1, paymentRequest.CardNumber, paymentRequest.CardExpiryMonth,
                paymentRequest.CardExpiryYear, paymentRequest.Amount, paymentRequest.Currency, paymentRequest.CVV);

            return CreatedAtAction(nameof(Create), paymentResponse);
        }
    }
}
