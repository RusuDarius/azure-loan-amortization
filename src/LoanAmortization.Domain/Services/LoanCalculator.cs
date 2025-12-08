using LoanAmortization.Domain.Models;

namespace LoanAmortization.Domain.Services;

// 0.0041 - monthly rate
// 120 total months
// 1000$
// 1000 * 0.0041666666666667 / (1 - (1 + 0.0041666666666667)^(-120)) = 10.61$
public sealed class LoanCalculator
{
    public IReadOnlyList<PaymentScheduleItem> CalculateSchedule(
        decimal principalLoanAmount,
        decimal annualInterestRate,
        DateTime startDate,
        int numberOfYears
    )
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(principalLoanAmount);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(numberOfYears);

        var totalMonths = numberOfYears * 12;
        var monthlyInterestRate = annualInterestRate / 12m;

        var monthlyPayment = CalculateMonthlyPayment(principalLoanAmount, monthlyInterestRate, totalMonths);

        var payments = new List<PaymentScheduleItem>(totalMonths);
        var remaining = principalLoanAmount;
        var paymentDate = GetFirstPaymentDate(startDate);

        for (int i = 0; i < totalMonths - 1; i++)
        {
            var interestPart = decimal.Round(remaining * monthlyInterestRate, 2); 
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
            var interestPart = decimal.Round(remaining * monthlyInterestRate, 2);
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
    // Formula: P * r / (1 - (1 + r)^(-n))
    // Where P = principalLoanAmount, r = monthlyInterestRate, n = totalMonths
    private static decimal CalculateMonthlyPayment(decimal principalLoanAmount, decimal monthlyInterestRate, int totalMonths)
    {
        var numerator = principalLoanAmount * monthlyInterestRate;
        var denominator = 1 - (decimal)Math.Pow(1 + (double)monthlyInterestRate, -totalMonths);

        return monthlyInterestRate == 0 ? principalLoanAmount / totalMonths : decimal.Round(numerator / denominator, 2);
    }
#endregion
}