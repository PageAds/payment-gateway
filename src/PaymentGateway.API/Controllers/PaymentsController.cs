using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using PaymentGateway.API.Extensions;
using PaymentGateway.API.Mappers;
using PaymentGateway.API.Models;
using PaymentGateway.Domain.Services;
using PaymentViewModel = PaymentGateway.API.Models.ViewModels.Payment;

namespace PaymentGateway.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentMapper paymentMapper;
        private readonly IPaymentService paymentService;
        private readonly IPaymentViewModelMapper paymentViewModelMapper;
        private readonly ILogger logger;

        public PaymentsController(
            IPaymentMapper paymentMapper,
            IPaymentService paymentService,
            IPaymentViewModelMapper paymentViewModelMapper,
            ILogger<PaymentsController> logger)
        {
            this.paymentMapper = paymentMapper;
            this.paymentService = paymentService;
            this.paymentViewModelMapper = paymentViewModelMapper;
            this.logger = logger;
        }

        /// <summary>
        /// Process a Payment
        /// </summary>
        /// <param name="paymentRequest"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(PaymentViewModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity, Type = typeof(ErrorResponse))]
        public async Task<IActionResult> Process(PaymentRequest paymentRequest)
        {
            var payment = paymentMapper.Map(paymentRequest);

            try
            {
                payment = await paymentService.CreatePayment(payment);
            }
            catch (ValidationException ex)
            {
                var errorResponse = new ErrorResponse
                {
                    Errors = ex.Errors.ToErrorProperties().ToList()
                };

                return UnprocessableEntity(errorResponse);
            }

            var paymentViewModel = paymentViewModelMapper.Map(payment);

            return CreatedAtAction(nameof(Process), paymentViewModel);
        }

        /// <summary>
        /// Retrieve a Payment by PaymentId
        /// </summary>
        /// <param name="paymentId"></param>
        /// <returns></returns>
        [HttpGet("{paymentId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaymentViewModel))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResponse))]
        public async Task<IActionResult> Get(long paymentId)
        {
            var payment = await paymentService.GetPayment(paymentId);

            if (payment == null)
            {
                logger.LogWarning($"Payment not found with Id: {paymentId}");
                return NotFound(new ErrorResponse { Errors = new List<Error> { new Error() { Message = "Payment not found" } } });
            }

            var paymentViewModel = paymentViewModelMapper.Map(payment);

            return Ok(paymentViewModel);
        }
    } 
}
