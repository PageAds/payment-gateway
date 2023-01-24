using FluentValidation;
using PaymentGateway.Mappers;
using PaymentGateway.Services.Services;
using PaymentGateway.Services.Validators;

namespace PaymentGateway
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddTransient<IPaymentMapper, PaymentMapper>();
            builder.Services.AddTransient<IPaymentService, PaymentService>();

            builder.Services.AddValidatorsFromAssemblyContaining<PaymentValidator>();

            builder.Services.AddControllers();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

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