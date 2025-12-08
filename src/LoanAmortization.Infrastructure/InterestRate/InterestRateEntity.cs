using Azure;
using Azure.Data.Tables;

namespace LoanAmortization.Infrastructure.InterestRate;

public sealed class InterestRateEntity : ITableEntity
{
    public string PartitionKey { get; set; } = "InterestRate";
    public string RowKey { get; set; } = "Default";
    public DateTimeOffset? Timestamp { get; set; }
    public ETag ETag { get; set; }

    public double AnnualRate { get; set; }
}