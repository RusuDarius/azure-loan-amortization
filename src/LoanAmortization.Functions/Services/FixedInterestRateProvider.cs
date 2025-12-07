using LoanAmortization.Application.Interfaces;

namespace LoanAmortization.Functions.Services;

public sealed class FixedInterestRateProvider : IInterestRateProvider
{
  //! Hardcoded 5% annual interest rate specified in the assignment
  //TODO: This is a temporary solution to get the annual interest rate. We need to implement a proper interest rate provider.
  private const decimal DefaultAnnualInterestRate = 0.05m;
  
  public Task<decimal> GetAnnualInterestRateAsync(CancellationToken cancellationToken = default)
  {
    return Task.FromResult(DefaultAnnualInterestRate);
  }
}