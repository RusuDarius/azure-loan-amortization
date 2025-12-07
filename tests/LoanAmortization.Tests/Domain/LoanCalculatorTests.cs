using LoanAmortization.Domain.Services;
using Xunit;

namespace LoanAmortization.Tests.Domain;

public class LoanCalculatorTests
{
    [Fact]
    public void Should_Produce_Expected_Number_Of_Payments()
    {
        var calculator = new LoanCalculator();

        var result = calculator.CalculateSchedule(
            principalLoanAmount: 50_000m,
            annualInterestRate: 0.05m,
            startDate: new DateTime(2025, 01, 01),
            numberOfYears: 5);

        Assert.Equal(60, result.Count);
    }

    [Fact]
    public void First_Payment_Should_Be_On_10th_Of_Following_Month()
    {
        var calculator = new LoanCalculator();
        var start = new DateTime(2025, 02, 15);

        var schedule = calculator.CalculateSchedule(
            principalLoanAmount: 10_000m,
            annualInterestRate: 0.05m,
            startDate: start,
            numberOfYears: 1);

        Assert.Equal(new DateTime(2025, 03, 10), schedule[0].PaymentDate);
    }

    [Fact]
    public void Final_Remaining_Should_Be_Zero()
    {
        var calculator = new LoanCalculator();
        
        var schedule = calculator.CalculateSchedule(
            principalLoanAmount: 100_000m,
            annualInterestRate: 0.05m,
            startDate: new DateTime(2025, 01, 01),
            numberOfYears: 2);

        Assert.Equal(0m, schedule[^1].RemainingAmount);
    }
}
