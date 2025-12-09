namespace LoanAmortization.Application.Interfaces;

public interface IInterestRateProvider
{
    Task<decimal> GetAnnualInterestRateAsync(CancellationToken cancellationToken = default);
}
