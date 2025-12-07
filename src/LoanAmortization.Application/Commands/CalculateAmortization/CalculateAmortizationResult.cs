namespace LoanAmortization.Application.Commands.CalculateAmortization;

public sealed class CalculateAmortizationResult
{
    public bool IsSuccess => Errors.Count == 0;

    public List<ValidationError> Errors { get; } = new List<ValidationError>();

    public IReadOnlyList<PaymentScheduleItemDto> Payments { get; init; } 
        = Array.Empty<PaymentScheduleItemDto>();
}
