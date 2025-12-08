namespace LoanAmortization.Application.Commands.CalculateAmortization;

public sealed class CalculateAmortizationCommand 
{
    public required decimal TotalSum { get; init; }
    public required DateTime StartDate { get; init; }
    public required int NumberOfYears { get; init; }
}