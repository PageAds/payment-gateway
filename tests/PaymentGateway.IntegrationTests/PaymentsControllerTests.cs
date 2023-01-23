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

            var payment = new PaymentRequest("5479630754337041", 4, 2027, 10.00m, "GBP", "123");

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

            var cardNumber = "5479630754337041";
            var cardExpiryMonth = 4;
            var cardExpiryYear = 2027;
            var amount = 10.00m;
            var currency = "GBP";
            var cvv = "123";

            var paymentRequest = new PaymentRequest(cardNumber, cardExpiryMonth, cardExpiryYear, amount, currency, cvv);

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
            paymentResponse.CardNumber.ShouldBe(cardNumber);
            paymentResponse.CardExpiryMonth.ShouldBe(cardExpiryMonth);
            paymentResponse.CardExpiryYear.ShouldBe(cardExpiryYear);
            paymentResponse.Amount.ShouldBe(amount);
            paymentResponse.Currency.ShouldBe(currency);
            paymentResponse.CVV.ShouldBe(cvv);
        }
    }
}