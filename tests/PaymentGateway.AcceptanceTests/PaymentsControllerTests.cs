using AutoFixture;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using PaymentGateway.AcceptanceTests.Mocks;
using PaymentGateway.Domain.Models;
using PaymentGateway.Infrastructure.HttpClients;
using PaymentGateway.Models;
using Shouldly;
using System.Net;
using System.Text;
using Xunit;
using PaymentViewModel = PaymentGateway.Models.ViewModels.Payment;

namespace PaymentGateway.IntegrationTests
{
    public class PaymentsControllerTests
    {
        private Fixture fixture = new Fixture();

        [Fact]
        public async Task Post_Payment_ReturnsHttpCreatedResponse()
        {
            // Arrange
            var application = new WebApplicationFactory<Program>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureTestServices(services =>
                    {
                        services.AddTransient<IBankApiClient, BankApiClientMock>();
                    });
                });

            var client = application.CreateClient();
            var payment = CreatePaymentRequest();
            var stringContent = new StringContent(JsonConvert.SerializeObject(payment), Encoding.UTF8, "application/json");

            // Act
            var response = await client.PostAsync("/payments", stringContent);

            // Assert
            response.ShouldNotBeNull();
            response.StatusCode.ShouldBe(HttpStatusCode.Created);
        }

        [Fact]
        public async Task Post_Payment_ReturnsCreatedPaymentInHttpResponseBody()
        {
            // Arrange
            var application = new WebApplicationFactory<Program>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureTestServices(services =>
                    {
                        services.AddTransient<IBankApiClient, BankApiClientMock>();
                    });
                });

            var client = application.CreateClient();
            var paymentRequest = CreatePaymentRequest();
            var stringContent = new StringContent(JsonConvert.SerializeObject(paymentRequest), Encoding.UTF8, "application/json");

            // Act
            var response = await client.PostAsync("/payments", stringContent);

            // Assert
            response.ShouldNotBeNull();
            response.Content.ShouldNotBeNull();
            var responseContentString = await response.Content.ReadAsStringAsync();

            var payment = JsonConvert.DeserializeObject<PaymentViewModel>(responseContentString);
            payment.ShouldNotBeNull();
            payment.Id.ShouldBeGreaterThan(0);
            payment.Status.ShouldNotBeNull();
            payment.CardNumber.ShouldNotBeNull();
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
        public async Task Post_PaymentWithAnInvalidCardNumber_ReturnsUnprocessableEntityErrorResponse(string cardNumber)
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

        [Theory]
        [InlineData(-1)] // invalid
        [InlineData(0)] // invalid
        [InlineData(13)] // too big
        public async Task Post_PaymentWithAnInvalidCardExpiryMonth_ReturnsUnprocessableEntityErrorResponse(int cardExpiryMonth)
        {
            // Arrange
            var application = new WebApplicationFactory<Program>();
            var client = application.CreateClient();
            var paymentRequest = CreatePaymentRequest(cardExpiryMonth: cardExpiryMonth);
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
            errorResponse.Errors.Single().FieldName.ShouldBe(nameof(PaymentRequest.CardExpiryMonth));
        }

        [Fact]
        public async Task Post_PaymentWithACardExpiryYearInThePast_ReturnsUnprocessableEntityErrorResponse()
        {
            // Arrange
            var application = new WebApplicationFactory<Program>();
            var client = application.CreateClient();

            var currentYear = DateTimeOffset.UtcNow.Year;
            var generator = fixture.Create<Generator<int>>();
            var cardExpiryYear = generator.Where(x => x >= 1900 && x < currentYear).Distinct().Take(1).Single();

            var paymentRequest = CreatePaymentRequest(cardExpiryYear: cardExpiryYear);
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
            errorResponse.Errors.Count.ShouldBe(2);
            errorResponse.Errors.Single(x => x.FieldName == nameof(Payment.CardExpiryYear)).ShouldNotBeNull();
            errorResponse.Errors.Single(x => x.FieldName == "CardExpiryMonth").ShouldNotBeNull();
        }

        [Fact]
        public async Task Post_PaymentWithACardExpiryInThePastThisYear_ReturnsUnprocessableEntityErrorResponse()
        {
            // Arrange
            var application = new WebApplicationFactory<Program>();
            var client = application.CreateClient();

            var currentMonth = DateTimeOffset.UtcNow.Month;
            if (currentMonth == 1) // January is a bad time to run this test since bank cards expire at the end of the month written on the card
                return;

            var generator = fixture.Create<Generator<int>>();
            var cardExpiryMonth = generator.Where(x => x >= 1 && x < currentMonth).Distinct().Take(1).Single();
            var cardExpiryYear = DateTimeOffset.UtcNow.Year;

            var paymentRequest = CreatePaymentRequest(cardExpiryMonth: cardExpiryMonth, cardExpiryYear: cardExpiryYear);
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
            errorResponse.Errors.Single().FieldName.ShouldBe("CardExpiryMonth");
        }

        [Theory]
        [InlineData(-1.00)]
        [InlineData(0)]
        public async Task Post_PaymentWithANegativeOrZeroAmount_ReturnsUnprocessableEntityErrorResponse(decimal amount)
        {
            var application = new WebApplicationFactory<Program>();
            var client = application.CreateClient();
            var paymentRequest = CreatePaymentRequest(amount: amount);
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
            errorResponse.Errors.Single().FieldName.ShouldBe(nameof(Payment.Amount));
        }

        [Theory]
        [InlineData("ABC")]
        [InlineData("XYZ")]
        public async Task Post_PaymentWithAnInvalidCurrency_ReturnsUnprocessableEntityErrorResponse(string currency)
        {
            var application = new WebApplicationFactory<Program>();
            var client = application.CreateClient();
            var paymentRequest = CreatePaymentRequest(currency: currency);
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
            errorResponse.Errors.Single().FieldName.ShouldBe(nameof(Payment.Currency));
        }

        [Theory]
        [InlineData("1")] // too short
        [InlineData("1234")] // too long
        [InlineData("!23")] // contains non numeric characters
        public async Task Post_PaymentWithAnInvalidCVV_ReturnsUnprocessableEntityErrorResponse(string cvv)
        {
            // Arrange
            var application = new WebApplicationFactory<Program>();
            var client = application.CreateClient();
            var paymentRequest = CreatePaymentRequest(cvv: cvv);
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
            errorResponse.Errors.Single().FieldName.ShouldBe(nameof(PaymentRequest.CVV));
        }

        [Fact]
        public async Task Post_Payment_ReturnsMaskedAccountNumber()
        {
            // Arrange
            var application = new WebApplicationFactory<Program>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureTestServices(services =>
                    {
                        services.AddTransient<IBankApiClient, BankApiClientMock>();
                    });
                });

            var client = application.CreateClient();
            var cardNumber = "1234567890123456";
            var expectedMaskedCardNumber = "************3456";
            var paymentRequest = CreatePaymentRequest(cardNumber: cardNumber);
            var stringContent = new StringContent(JsonConvert.SerializeObject(paymentRequest), Encoding.UTF8, "application/json");

            // Act
            var response = await client.PostAsync("/payments", stringContent);

            // Assert
            response.ShouldNotBeNull();
            response.Content.ShouldNotBeNull();
            var responseContentString = await response.Content.ReadAsStringAsync();

            var payment = JsonConvert.DeserializeObject<PaymentViewModel>(responseContentString);
            payment.ShouldNotBeNull();
            payment.CardNumber.ShouldBe(expectedMaskedCardNumber);
        }

        [Fact]
        public async Task Get_Payment_ReturnsHttpOKResponse()
        {
            // Arrange
            var application = new WebApplicationFactory<Program>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureTestServices(services =>
                    {
                        services.AddSingleton<IBankApiClient, BankApiClientMock>();
                    });
                });

            var client = application.CreateClient();
            var paymentRequest = CreatePaymentRequest();
            var stringContent = new StringContent(JsonConvert.SerializeObject(paymentRequest), Encoding.UTF8, "application/json");

            // Act
            var postPaymentResponse = await client.PostAsync("/payments", stringContent);
            var postPaymentResponseContentString = await postPaymentResponse.Content.ReadAsStringAsync();

            var payment = JsonConvert.DeserializeObject<PaymentViewModel>(postPaymentResponseContentString);
            payment.ShouldNotBeNull();

            var getPaymentResponse = await client.GetAsync($"/payments/{payment.Id}");

            // Assert
            getPaymentResponse.ShouldNotBeNull();
            getPaymentResponse.StatusCode.ShouldBe(HttpStatusCode.OK);
            var getPaymentResponseContentString = await getPaymentResponse.Content.ReadAsStringAsync();

            var getPayment = JsonConvert.DeserializeObject<PaymentViewModel>(getPaymentResponseContentString);
            getPayment.ShouldNotBeNull();
            getPaymentResponse.StatusCode.ShouldBe(HttpStatusCode.OK);
            getPayment.Id.ShouldBe(payment.Id);
            getPayment.Status.ShouldNotBeNull();
            getPayment.CardNumber.ShouldBe(payment.CardNumber);
            getPayment.CardExpiryMonth.ShouldBe(payment.CardExpiryMonth);
            getPayment.CardExpiryYear.ShouldBe(payment.CardExpiryYear);
            getPayment.Amount.ShouldBe(payment.Amount);
            getPayment.Currency.ShouldBe(payment.Currency);
            getPayment.CVV.ShouldBe(payment.CVV);
        }

        [Fact]
        public async Task Get_PaymentIsNotFound_ReturnsHttpNotFoundResponse()
        {
            // Arrange
            var application = new WebApplicationFactory<Program>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureTestServices(services =>
                    {
                        services.AddTransient<IBankApiClient, BankApiClientMock>();
                    });
                });

            var client = application.CreateClient();

            // Act
            var getPaymentResponse = await client.GetAsync($"/payments/1");

            // Assert
            getPaymentResponse.ShouldNotBeNull();
            getPaymentResponse.StatusCode.ShouldBe(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Get_Payment_ReturnsMaskedAccountNumber()
        {
            // Arrange
            var application = new WebApplicationFactory<Program>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureTestServices(services =>
                    {
                        services.AddSingleton<IBankApiClient, BankApiClientMock>();
                    });
                });

            var client = application.CreateClient();
            var cardNumber = "1234567890123456";
            var expectedMaskedCardNumber = "************3456";
            var paymentRequest = CreatePaymentRequest(cardNumber: cardNumber);
            var stringContent = new StringContent(JsonConvert.SerializeObject(paymentRequest), Encoding.UTF8, "application/json");

            // Act
            var postPaymentResponse = await client.PostAsync("/payments", stringContent);
            var postPaymentResponseContentString = await postPaymentResponse.Content.ReadAsStringAsync();

            var payment = JsonConvert.DeserializeObject<PaymentViewModel>(postPaymentResponseContentString);
            payment.ShouldNotBeNull();

            var getPaymentResponse = await client.GetAsync($"/payments/{payment.Id}");

            // Assert
            getPaymentResponse.ShouldNotBeNull();
            getPaymentResponse.StatusCode.ShouldBe(HttpStatusCode.OK);
            var getPaymentResponseContentString = await getPaymentResponse.Content.ReadAsStringAsync();

            var getPayment = JsonConvert.DeserializeObject<PaymentViewModel>(getPaymentResponseContentString);
            getPayment.ShouldNotBeNull();
            getPaymentResponse.StatusCode.ShouldBe(HttpStatusCode.OK);
            getPayment.CardNumber.ShouldBe(expectedMaskedCardNumber);
        }

        private PaymentRequest CreatePaymentRequest(string cardNumber = "5479630754337041", int cardExpiryMonth = 4, int cardExpiryYear = 2027, decimal amount = 10.00m, string currency = "GBP", string cvv = "123")
        {
            return new PaymentRequest(cardNumber, cardExpiryMonth, cardExpiryYear, amount, currency, cvv);
        }
    }
}