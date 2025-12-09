namespace LoanAmortization.Infrastructure.Seeding;

public interface IStorageSeeder
{
  Task SeedAsync(CancellationToken cancellationToken = default);
}
