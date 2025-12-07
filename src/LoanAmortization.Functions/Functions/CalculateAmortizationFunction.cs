using LoanAmortization.Application.Commands.CalculateAmortization;
using Microsoft.Extensions.Logging;

namespace LoanAmortization.Functions.Functions;

public sealed class CalculateAmortizationFunction 
{
  private readonly CalculateAmortizationHandler _calculateAmortizationHandler;
  private readonly ILogger<CalculateAmortizationFunction> _logger;

  public CalculateAmortizationFunction(CalculateAmortizationHandler calculateAmortizationHandler, ILogger<CalculateAmortizationFunction> logger)
    {
      _calculateAmortizationHandler = calculateAmortizationHandler;
      _logger = logger;
    }
}