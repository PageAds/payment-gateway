using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using PaymentGateway.Application.Services;
using PaymentGateway.Domain.Services;
using PaymentGateway.Domain.Validators;
using PaymentGateway.Filters;
using PaymentGateway.Infrastructure.HttpClients;
using PaymentGateway.Infrastructure.Mappers;
using PaymentGateway.Mappers;
using System.Reflection;

namespace PaymentGateway
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddTransient<Mappers.IPaymentMapper, Mappers.PaymentMapper>();
            builder.Services.AddTransient<IPaymentService, PaymentService>();
            builder.Services.AddTransient<IBankApiClient, BankApiClient>();
            builder.Services.AddTransient<IBankApiPaymentRequestMapper, BankApiPaymentRequestMapper>();
            builder.Services.AddTransient<IPaymentViewModelMapper, PaymentViewModelMapper>();
            builder.Services.AddTransient<Infrastructure.Mappers.IPaymentMapper, Infrastructure.Mappers.PaymentMapper>();

            builder.Services.AddSingleton<IMemoryCache, MemoryCache>();

            builder.Services.AddHttpClient<IBankApiClient, BankApiClient>((httpClient) =>
            {
                httpClient.BaseAddress = new Uri(builder.Configuration["BankApiBaseUrl"]);
            });

            builder.Services.AddValidatorsFromAssemblyContaining<PaymentValidator>();

            builder.Services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            builder.Services.AddControllers(options =>
            {
                options.Filters.Add<ValidationFilter>();
            });

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
            });

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}