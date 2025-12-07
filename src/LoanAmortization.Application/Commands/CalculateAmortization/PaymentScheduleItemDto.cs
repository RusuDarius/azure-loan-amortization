namespace LoanAmortization.Application.Commands.CalculateAmortization;

public sealed class PaymentScheduleItemDto
{
    public DateTime PaymentDate { get; init; }
    public decimal PaymentAmount { get; init; }
    public decimal RemainingAmount { get; init; }
}