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

# Running the tests
## Instructions
1. Clone the repository to your local machine using Git
2. Open an instance of a terminal (e.g. Windows Command Prompt), navigate to the root of the repository and run the following command:

    `dotnet test`

3. This will execute the tests and output a result indicating the success or failure of the tests

# Assumptions made
- No requirement for the Payment Gateway to store the Payment entity
    - This is important if an additional bank needs to be integrated since the Id of the Payment entity is currently derived from a single bank. 
    - To elaborate further, in a world where the Payment Gateway is integrating with multiple banks then storing the Payment entity would allow for an internal Payment Id (managed by the Payment Gateway) to be generated that can be exposed to the client. The external Payment Id (managed by the Bank) can be saved against the Payment entity which we store for the purpose of retrieving the Payment from the banks API.
    - This could also be useful if the banks API is rate limited (as a fallback we can load the Payment entity which has been stored prior).
- Card number length validation is between 13 and 19 characters to support all bank cards listed [here](https://www.validcreditcardnumber.com/)

# Areas for improvement
- Add RegEx for more accurate validation on card numbers.
- Access to the Payment Gateway is currently not authenticated. A Payment Gateway identity server could be implemented to issue access tokens (e.g. as part of [OAuth2](https://oauth.net/2/) protocol). This identity server could then be consumed by the Payment Gateway service to validate incoming access tokens.
- Configure HTTPS
