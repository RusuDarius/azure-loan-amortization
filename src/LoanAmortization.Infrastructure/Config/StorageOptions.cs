namespace LoanAmortization.Infrastructure.Config;

public sealed class StorageOptions
{
    public required string ConnectionString { get; init; }
    public string InterestRateTableName { get; set; } = "LoanSettings";
}