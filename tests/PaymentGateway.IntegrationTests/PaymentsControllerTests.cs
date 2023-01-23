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
    }
}