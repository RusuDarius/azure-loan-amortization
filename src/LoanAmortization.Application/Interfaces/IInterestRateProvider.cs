namespace LoanAmortization.Application.Interfaces;

public interface IInterestRateProvider
{
    /// <summary>
    /// Returns the annual interest rate as a decimal (e.g. 0.05 for 5% per year).
    /// </summary>
    Task<decimal> GetAnnualInterestRateAsync(CancellationToken cancellationToken = default);
}
