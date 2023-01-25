# Running the service
## Prerequisites
- [Git](https://git-scm.com/downloads)
- [.NET 6 SDK](https://dotnet.microsoft.com/en-us/download)

## Instructions
1. Clone the repository to your local machine using Git
2. Open two instances of a terminal (e.g. Windows Command Prompt), navigate to the root of the repository and run the following commands:
    -  `dotnet run --project src\BankSimulator.API`
    -  `dotnet watch run --project src\PaymentGateway.API`
3. The last command will open a browser to the Swagger UI of the Payment Gateway API where you can execute two endpoints:
    - `POST /payments`
    - `GET /payments/{paymentId}`

4. Alternatively you can make API requests with any client of your choice (e.g. Postman) to the URL that the service is running on (this is configured by default to be http://localhost:5201/)