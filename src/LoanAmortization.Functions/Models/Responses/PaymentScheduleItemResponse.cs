namespace LoanAmortization.Functions.Models.Responses;

public sealed class PaymentScheduleItemResponse
{
    public DateTime PaymentDate { get; set; }
    public decimal PaymentAmount { get; set; }
    public decimal RemainingAmount { get; set; }
}