using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using PaymentGateway.Domain.Models;
using PaymentGateway.Extensions;
using PaymentGateway.Mappers;
using PaymentGateway.Models;
using PaymentGateway.Services.Services;

namespace PaymentGateway.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentMapper paymentMapper;
        private readonly IPaymentService paymentService;
         
        public PaymentsController(
            IPaymentMapper paymentMapper,
            IPaymentService paymentService)
        {
            this.paymentMapper = paymentMapper;
            this.paymentService = paymentService;
        }

        /// <summary>
        /// Processes and stores a Payment
        /// </summary>
        /// <param name="paymentRequest"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity, Type = typeof(ErrorResponse))]
        public async Task<IActionResult> Process(PaymentRequest paymentRequest)
        {
            var payment = paymentMapper.Map(paymentRequest);

            try
            {
                await paymentService.CreatePayment(payment);
            }
            catch (ValidationException ex)
            {
                var errorResponse = new ErrorResponse
                {
                    Errors = ex.Errors.ToErrorProperties().ToList()
                };

                return UnprocessableEntity(errorResponse);
            }

            return CreatedAtAction(nameof(Process), payment);
        }
        /// <summary>
        /// Retrieve a Payment by PaymentId
        /// </summary>
        /// <param name="paymentId"></param>
        /// <returns></returns>
        [HttpGet("{paymentId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Payment))]
        public async Task<IActionResult> Get(long paymentId)
        {
            var payment = await paymentService.GetPayment(paymentId);
            return Ok(payment);
        }
    } 
}
