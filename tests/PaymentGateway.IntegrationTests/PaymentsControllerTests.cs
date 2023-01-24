using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using PaymentGateway.Domain.Models;
using PaymentGateway.Models;
using Shouldly;
using System.Net;
using System.Text;
using Xunit;

namespace PaymentGateway.IntegrationTests
{
    public class PaymentsControllerTests
    {
        [Fact]
        public async Task Post_WhenAPaymentIsCreated_ReturnsHttpCreatedResponse()
        {
            // Arrange
            var application = new WebApplicationFactory<Program>();

            var client = application.CreateClient();

            var payment = CreatePaymentRequest();

            var stringContent = new StringContent(JsonConvert.SerializeObject(payment), Encoding.UTF8, "application/json");

            // Act
            var response = await client.PostAsync("/payments", stringContent);

            // Assrt
            response.ShouldNotBeNull();
            response.StatusCode.ShouldBe(HttpStatusCode.Created);
        }

        [Fact]
        public async Task Post_WhenAPaymentIsCreated_ReturnsCreatedPaymentInHttpResponseBody()
        {
            // Arrange
            var application = new WebApplicationFactory<Program>();

            var client = application.CreateClient();

            var paymentRequest = CreatePaymentRequest();

            var stringContent = new StringContent(JsonConvert.SerializeObject(paymentRequest), Encoding.UTF8, "application/json");

            // Act
            var response = await client.PostAsync("/payments", stringContent);

            // Assrt
            response.ShouldNotBeNull();
            response.Content.ShouldNotBeNull();
            var responseContentString = await response.Content.ReadAsStringAsync();

            var payment = JsonConvert.DeserializeObject<Payment>(responseContentString);
            payment.ShouldNotBeNull();
            payment.Id.ShouldBeGreaterThan(0);
            payment.CardNumber.ShouldBe(paymentRequest.CardNumber);
            payment.CardExpiryMonth.ShouldBe(paymentRequest.CardExpiryMonth);
            payment.CardExpiryYear.ShouldBe(paymentRequest.CardExpiryYear);
            payment.Amount.ShouldBe(paymentRequest.Amount);
            payment.Currency.ShouldBe(paymentRequest.Currency);
            payment.CVV.ShouldBe(paymentRequest.CVV);
        }

        [Theory]
        [InlineData("1234")] // too short
        [InlineData("123456789123456789123456789")] // too long
        [InlineData("!23456abc1234567")] // contains non numeric characters
        public async Task Post_WhenAPaymentIsCreatedWithAnInvalidCardNumber_ReturnsUnprocessableEntityErrorResponse(string cardNumber)
        {
            // Arrange
            var application = new WebApplicationFactory<Program>();

            var client = application.CreateClient();

            var paymentRequest = CreatePaymentRequest(cardNumber: cardNumber);

            var stringContent = new StringContent(JsonConvert.SerializeObject(paymentRequest), Encoding.UTF8, "application/json");

            // Act
            var response = await client.PostAsync("/payments", stringContent);

            // Assert
            response.ShouldNotBeNull();
            response.StatusCode.ShouldBe(HttpStatusCode.UnprocessableEntity);
            response.Content.ShouldNotBeNull();
            var responseContentString = await response.Content.ReadAsStringAsync();

            var errorResponse = JsonConvert.DeserializeObject<ErrorResponse>(responseContentString);
            errorResponse.ShouldNotBeNull();
            errorResponse.Errors.Count.ShouldBe(1);
            errorResponse.Errors.Single().FieldName.ShouldBe(nameof(PaymentRequest.CardNumber));
        }

        private PaymentRequest CreatePaymentRequest(string cardNumber = "5479630754337041", int cardExpiryMonth = 4, int cardExpiryYear = 2027, decimal amount = 10.00m, string currency = "GBP", string cvv = "123")
        {
            return new PaymentRequest(cardNumber, cardExpiryMonth, cardExpiryYear, amount, currency, cvv);
        }
    }
}