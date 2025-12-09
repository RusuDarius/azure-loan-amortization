using Azure;
using Azure.Data.Tables;
using LoanAmortization.Infrastructure.InterestRate;

namespace LoanAmortization.Infrastructure.Seeding;

public sealed class StorageSeeder : IStorageSeeder
{
  private readonly TableClient _tableClient;
  private const double DefaultInterestRate = 0.05;
  public StorageSeeder(TableClient tableClient)
  {
    _tableClient = tableClient;
  }
  public async Task SeedAsync(CancellationToken cancellationToken = default)
  {
    await _tableClient.CreateIfNotExistsAsync(cancellationToken);

    var interestRateEntity = new InterestRateEntity
    {
      PartitionKey = "InterestRate",
      RowKey = "Default",
      AnnualRate = DefaultInterestRate
    };

    try
    {
      var response = await _tableClient.GetEntityIfExistsAsync<InterestRateEntity>(
        partitionKey: "InterestRate",
        rowKey: "Default",
        cancellationToken: cancellationToken);
    }
    catch (RequestFailedException) {}

    await _tableClient.AddEntityAsync(interestRateEntity, cancellationToken);
  }
}
