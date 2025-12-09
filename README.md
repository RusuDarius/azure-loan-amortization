# Loan Amortization Calculator

This solution implements a loan amortization calculator exposed as an Azure Function, 
using the isolated worker model.  
It calculates a monthly amortization schedule for a given principal, start date, and duration,  
using an interest rate stored in Azure Table Storage.

The main part is the formula used which is the annuity formula, used by banks worldwide:
```
MonthlyPayment = P × [ r(1 + r)^n ] / [ (1 + r)^n – 1 ]

P = Principal loan amount
r = Monthly rate
n = Total number of months
```

Requirements like the 5% rate and monthly payment date (10th of each month) have also been followed.

## Setup and Requirements for the Local environment

```
.NET SDK / version 8.0
Azure Function Core Tools / v4
Docker to run Azurite emulator (can run with Azurite locally.. just personal preference here)
```

### To run locally:

Pull and run the docker image inside of a container on the default ports for the blobs, queues and tables

```sh
docker run -p 10000:10000 -p 10001:10001 -p 10002:10002 \ mcr.microsoft.com/azure-storage/azurite
```

It will use the default connection string for local/dev:
```
UseDevelopmentStorage=true
```

The seeding script will populate a LoanSettings table with a singular entry that will have an annual interest rate set to 5%.

```sh
dotnet build 
cd src/LoanAmortization.Functions
func start
```

The exposed endpoint will be available at /calculate-amortization

### Sample request and expected response

Through Postman/Insomnia or something similar

Request:
```json
{
  "TotalSum": 100000,
  "StartDate": "2025-02-01",
  "NumberOfYears": 10
}
```

Response:

```json
"payments":
  [
    {
      "PaymentDate": "2025-02-10T00:00:00",
      "PaymentAmount": 87.92,
      "RemainingAmount": 920.23
    },
    ...
  ]
```
