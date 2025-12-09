using LoanAmortization.Application.Interfaces;

namespace LoanAmortization.Functions.Services;

public sealed class FixedInterestRateProvider : IInterestRateProvider
{
  private const decimal DefaultAnnualInterestRate = 0.05m;
  
  public Task<decimal> GetAnnualInterestRateAsync(CancellationToken cancellationToken = default)
  {
    return Task.FromResult(DefaultAnnualInterestRate);
  }
}