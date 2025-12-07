using LoanAmortization.Application.Interfaces;
using LoanAmortization.Domain.Models;
using LoanAmortization.Domain.Services;

namespace LoanAmortization.Application.Commands.CalculateAmortization;

public sealed class CalculateAmortizationHandler 
{
  private readonly LoanCalculator _loanCalculator;
  private readonly IInterestRateProvider _interestRateProvider;

  // The 5% requirement for annual interest rate
  private const decimal DefaultInterestRate = 0.05m;

  public CalculateAmortizationHandler(LoanCalculator loanCalculator, IInterestRateProvider interestRateProvider)
  {
    _loanCalculator = loanCalculator;
    _interestRateProvider = interestRateProvider;
  }

  public async Task<CalculateAmortizationResult> HandleAsync(CalculateAmortizationCommand command, CancellationToken cancellationToken = default)
  {
    var result = new CalculateAmortizationResult();

    ValidateCommand(command, result);
  
    if (!result.IsSuccess)
    {
      return result;
    }

    decimal annualInterestRate;
    try
    {
      //TODO
      annualInterestRate = await _interestRateProvider.GetAnnualInterestRateAsync(cancellationToken);

      if(annualInterestRate <= 0)
      {
        annualInterestRate = DefaultInterestRate;
      }
    }
    catch 
    {
      //! This is to keep behaviour consistent when the interest rate provider fails (demo purposes only)
      annualInterestRate = DefaultInterestRate;
    }

    IReadOnlyList<PaymentScheduleItem> paymentSchedule = 
      _loanCalculator.CalculateSchedule(
        principalLoanAmount: command.TotalAmount, 
        annualInterestRate: annualInterestRate,
        startDate: command.StartDate,
        numberOfYears: command.NumberOfYears
      );
      
      result = new CalculateAmortizationResult()
      {
        Payments = paymentSchedule.Select(p => new PaymentScheduleItemDto 
            {
                PaymentDate = p.PaymentDate,
                PaymentAmount = p.PaymentAmount,
                RemainingAmount = p.RemainingAmount
            })
            .ToList()
      };

    return result;
  }

#region Helper Methods
  private static void ValidateCommand(CalculateAmortizationCommand command, CalculateAmortizationResult result)
  {
    if (command.TotalAmount <= 0)
    {
      result.Errors.Add(new ValidationError { ErrorField = nameof(command.TotalAmount), ErrorMessage = "Total amount must be greater than 0" });
    }

    if (command.StartDate == default)
    {
      result.Errors.Add(new ValidationError { ErrorField = nameof(command.StartDate), ErrorMessage = "Start date must be a valid date" });
    }

    if(command.NumberOfYears <= 0)
    {
      result.Errors.Add(new ValidationError { ErrorField = nameof(command.NumberOfYears), ErrorMessage = "Number of years must be greater than 0" });
    }
  }
#endregion
}