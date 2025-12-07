namespace LoanAmortization.Application.Commands.CalculateAmortization;

public sealed class ValidationError
{
  public string ErrorField { get; init; } = string.Empty;
  public string ErrorMessage { get; init; } = string.Empty;
}
