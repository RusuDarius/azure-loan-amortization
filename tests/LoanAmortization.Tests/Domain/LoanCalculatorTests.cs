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

    [Fact]
    public void Monthly_Payment_Should_Match_Expected_Formula_Result()
    {
        var calculator = new LoanCalculator();
        var principal = 1000m;
        var monthlyRate = 0.0041666666666667m;
        var annualRate = monthlyRate * 12m; 
        var totalMonths = 120; 

        var schedule = calculator.CalculateSchedule(
            principalLoanAmount: principal,
            annualInterestRate: annualRate,
            startDate: new DateTime(2025, 01, 01),
            numberOfYears: totalMonths / 12);

        var monthlyPayment = schedule[0].PaymentAmount;
        Assert.Equal(10.61m, monthlyPayment);
    }

    [Fact]
    public void Total_Payments_Should_Equal_Principal_Plus_Interest()
    {
        var calculator = new LoanCalculator();
        var principal = 100_000m;
        var annualRate = 0.05m;
        var years = 10;

        var schedule = calculator.CalculateSchedule(
            principalLoanAmount: principal,
            annualInterestRate: annualRate,
            startDate: new DateTime(2025, 01, 01),
            numberOfYears: years);

        var totalPaid = schedule.Sum(p => p.PaymentAmount);
        var totalInterest = totalPaid - principal;

        decimal remaining = principal;
        decimal totalInterestCalculated = 0m;
        
        for (int i = 0; i < schedule.Count - 1; i++)
        {
            var payment = schedule[i];
            var interestPart = decimal.Round(remaining * (annualRate / 12m), 2);
            totalInterestCalculated += interestPart;
            remaining -= payment.PaymentAmount - interestPart;
        }

        if (schedule.Count > 0)
        {
            var lastPayment = schedule[^1];
            var lastInterest = lastPayment.PaymentAmount - remaining;
            totalInterestCalculated += lastInterest;
        }

        Assert.Equal(principal + totalInterest, totalPaid);
        
        Assert.True(Math.Abs(totalInterest - totalInterestCalculated) < 1m, 
            $"Total interest mismatch: Expected ~{totalInterest}, Calculated {totalInterestCalculated}");
    }

    [Theory]
    [InlineData(100_000.0, 0.05, 10)]
    [InlineData(50_000.0, 0.03, 5)]
    [InlineData(25_000.0, 0.07, 15)]
    public void Monthly_Payment_Should_Produce_Balanced_Schedule(double principal, double annualRate, int years)
    {
        var calculator = new LoanCalculator();
        
        var schedule = calculator.CalculateSchedule(
            principalLoanAmount: (decimal)principal,
            annualInterestRate: (decimal)annualRate,
            startDate: new DateTime(2025, 01, 01),
            numberOfYears: years);

        // Last balance should be zero
        Assert.Equal(0m, schedule[^1].RemainingAmount);

        Assert.All(schedule, p => Assert.True(p.PaymentAmount > 0));
        Assert.All(schedule, p => Assert.True(p.RemainingAmount >= 0));

        for (int i = 1; i < schedule.Count; i++)
        {
            Assert.True(schedule[i].RemainingAmount <= schedule[i - 1].RemainingAmount,
                $"Remaining balance should decrease: {schedule[i - 1].RemainingAmount} -> {schedule[i].RemainingAmount}");
        }
    }
}
