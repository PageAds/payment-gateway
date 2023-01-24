using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
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

            var paymentResponse = JsonConvert.DeserializeObject<PaymentResponse>(responseContentString);
            paymentResponse.ShouldNotBeNull();
            paymentResponse.Id.ShouldBeGreaterThan(0);
            paymentResponse.CardNumber.ShouldBe(paymentRequest.CardNumber);
            paymentResponse.CardExpiryMonth.ShouldBe(paymentRequest.CardExpiryMonth);
            paymentResponse.CardExpiryYear.ShouldBe(paymentRequest.CardExpiryYear);
            paymentResponse.Amount.ShouldBe(paymentRequest.Amount);
            paymentResponse.Currency.ShouldBe(paymentRequest.Currency);
            paymentResponse.CVV.ShouldBe(paymentRequest.CVV);
        }

        [Theory]
        [InlineData("1234")] // too short
        [InlineData("123456789123456789123456789")] // too long
        [InlineData("!23456abc1234567")] // contains non numeric characters
        public async Task Post_WhenAPaymentIsCreatedWithAnInvalidCardNumber_ReturnsUnprocessableEntityResponse(string cardNumber)
        {
            // Arrange
            var application = new WebApplicationFactory<Program>();

            var client = application.CreateClient();

            var paymentRequest = CreatePaymentRequest(cardNumber: cardNumber);

            var stringContent = new StringContent(JsonConvert.SerializeObject(paymentRequest), Encoding.UTF8, "application/json");

            // Act
            var response = await client.PostAsync("/payments", stringContent);

            // Assrt
            response.ShouldNotBeNull();
            response.StatusCode.ShouldBe(HttpStatusCode.UnprocessableEntity);
        }

        private PaymentRequest CreatePaymentRequest(string cardNumber = "5479630754337041", int cardExpiryMonth = 4, int cardExpiryYear = 2027, decimal amount = 10.00m, string currency = "GBP", string cvv = "123")
        {
            return new PaymentRequest(cardNumber, cardExpiryMonth, cardExpiryYear, amount, currency, cvv);
        }
    }
}