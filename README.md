# Running the service
## Prerequisites
- [Git](https://git-scm.com/downloads)
- [.NET 6 SDK](https://dotnet.microsoft.com/en-us/download)

## Instructions
1. Clone the repository to your local machine using Git.
2. Open two instances of a terminal (e.g. Windows Command Prompt), navigate to the root of the repository and run the following commands (each command should be run in a separate terminal instance):
    -  `dotnet run --project src\BankSimulator.API`
    -  `dotnet watch run --project src\PaymentGateway.API`
3. The last command will open a browser to the Swagger UI of the Payment Gateway API where you can execute two endpoints:
    - `POST /payments`
    - `GET /payments/{paymentId}`
4. Alternatively you can make API requests with any client of your choice (e.g. Postman) to the URL that the service is running on (this is configured by default to be http://localhost:5201/).

# Running the tests
## Prerequisites
- [Git](https://git-scm.com/downloads)
- [.NET 6 SDK](https://dotnet.microsoft.com/en-us/download)

## Instructions
1. Clone the repository to your local machine using Git.
2. Open an instance of a terminal (e.g. Windows Command Prompt), navigate to the root of the repository and run the following command:
    `dotnet test`
3. This will execute the tests and output a result indicating the success or failure of the tests.

# Assumptions made
- No requirement for the Payment Gateway to store the Payment entity.
    - This is important if an additional bank needs to be integrated since the identifier of the Payment entity is currently derived from a single bank. 
    - To elaborate further, in a world where the Payment Gateway is integrating with multiple banks then storing the Payment entity would allow for an internal Payment identifier (managed by the Payment Gateway) to be generated that can be exposed to the client. The external Payment identifier (managed by the Bank) can be saved against the Payment entity which we store for the purpose of retrieving the Payment from the banks API.
    - This could also be useful if the banks API is rate limited (as a fallback we can load the Payment entity which has been stored prior).
- Card number length validation is between 12 and 19 characters to support all bank cards listed [here](https://www.validcreditcardnumber.com/) (under the "How many digits in a Credit Card Number?" section).

# Areas for improvement
- Add RegEx for more accurate validation of card numbers.
- Access to the Payment Gateway is currently not authenticated. A Payment Gateway identity server could be implemented to issue access tokens (e.g. as part of [OAuth2](https://oauth.net/2/) protocol). This identity server could then be consumed by the Payment Gateway service to validate incoming access tokens.
- Configure HTTPS.

# Cloud technologies
Perform the following if ever required to host the application in the cloud:
- In Azure (what I am most familiar with), create the following resources for each API service, per environment:
    - [App Service](https://azure.microsoft.com/en-us/products/app-service/#overview) - to host the service.
    - [Application Insights](https://learn.microsoft.com/en-us/azure/azure-monitor/app/app-insights-overview?tabs=net) - for application telemetry (observability).
        - After creating this resource an `Instrumentation Key` will be issued, the value of this will need to be added to the App Services application settings with the name `APPINSIGHTS_INSTRUMENTATIONKEY`.
    - [Key Vault](https://azure.microsoft.com/en-us/products/key-vault/) - should any sensitive configuration needs to be stored (e.g. credentials to access the banks API).
    - [Azure Cache for Redis](https://azure.microsoft.com/en-us/products/cache/) - in case the app service is scaled out across multiple instances
        - This could be utilised by the Bank Simulator API since it's currently storing/reading from an in memory cache, but it could be changed to use a distributed cache instead.
- Within the Git repository, create a YAML pipeline to build/test and deploy the service to a pre-production environment (with an approval step for production).
- Use a logging provider such as [Microsoft.Extensions.Logging.ApplicationInsights](https://www.nuget.org/packages/Microsoft.Extensions.Logging.ApplicationInsights) to ensure application logs can be monitored when hosted in Azure.
