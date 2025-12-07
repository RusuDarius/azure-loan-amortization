using LoanAmortization.Domain.Models;

namespace LoanAmortization.Domain.Services;

public sealed class LoanCalculator
{
    public static IReadOnlyList<PaymentScheduleItem> CalculateSchedule(
        decimal principal,
        decimal annualInterestRate,
        DateTime startDate,
        int numberOfYears
    )
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(principal);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(numberOfYears);

        var totalMonths = numberOfYears * 12;
        var monthlyRate = annualInterestRate / 12m;

        var monthlyPayment = CalculateMonthlyPayment(principal, monthlyRate, totalMonths);

        var payments = new List<PaymentScheduleItem>(totalMonths);
        var remaining = principal;
        var paymentDate = GetFirstPaymentDate(startDate);

        for (int i = 0; i < totalMonths - 1; i++)
        {
            var interestPart = decimal.Round(remaining * monthlyRate, 2);
            var principalPart = monthlyPayment - interestPart;

            remaining -= principalPart;
            if (remaining < 0)
                remaining = 0;

            payments.Add(
                new PaymentScheduleItem
                {
                    PaymentDate = paymentDate,
                    PaymentAmount = monthlyPayment,
                    RemainingAmount = remaining,
                }
            );

            paymentDate = paymentDate.AddMonths(1);
        }

        // Handle the last payment separately to ensure exact remaining balance
        if (totalMonths > 0)
        {
            var interestPart = decimal.Round(remaining * monthlyRate, 2);
            var principalPart = remaining;
            var finalPayment = principalPart + interestPart;

            payments.Add(
                new PaymentScheduleItem
                {
                    PaymentDate = paymentDate,
                    PaymentAmount = finalPayment,
                    RemainingAmount = 0,
                }
            );
        }

        return payments;
    }

#region Helper Methods
    private static DateTime GetFirstPaymentDate(DateTime startDate)
    {
        // The 10th of next month
        var year = startDate.Month == 12 ? startDate.Year + 1 : startDate.Year;
        var month = startDate.Month == 12 ? 1 : startDate.Month + 1;

        return new DateTime(year, month, 10);
    }

    // Use the monthly amortized payment formula to calculate the monthly payment
    private static decimal CalculateMonthlyPayment(decimal principal, decimal monthlyRate, int totalMonths) => 
        monthlyRate == 0 ? principal / totalMonths
            : decimal.Round(principal * monthlyRate / (1 - (decimal)Math.Pow(1 + (double)monthlyRate, -totalMonths)), 2);

#endregion
}