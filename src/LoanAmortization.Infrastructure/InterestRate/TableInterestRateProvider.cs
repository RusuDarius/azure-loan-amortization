using Azure;
using Azure.Data.Tables;
using LoanAmortization.Application.Interfaces;

namespace LoanAmortization.Infrastructure.InterestRate;

public sealed class TableInterestRateProvider : IInterestRateProvider
{
    private readonly TableClient _tableClient;
    private const decimal DefaultInterestRate = 0.05m;

    public TableInterestRateProvider(TableClient tableClient)
    {
      _tableClient = tableClient;
    }

    public async Task<decimal> GetAnnualInterestRateAsync(CancellationToken cancellationToken = default)
    {
        try
        {
          var response = await _tableClient.GetEntityIfExistsAsync<InterestRateEntity>(
          partitionKey: "InterestRate", 
          rowKey: "Default", 
          cancellationToken: cancellationToken);

          if (response.Value is not null)
          {
            return (decimal)response.Value.AnnualRate;
          }

          return DefaultInterestRate;
        }
        catch (RequestFailedException)
        {
          //! This is to keep behaviour consistent when the interest rate provider fails (demo purposes only)
          return DefaultInterestRate;
        }
    }
}
