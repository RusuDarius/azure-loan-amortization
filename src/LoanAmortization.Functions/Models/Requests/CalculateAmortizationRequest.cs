namespace LoanAmortization.Functions.Models.Requests;

public sealed class CalculateAmortizationRequest 
{
    public decimal TotalSum { get; set; }
    public DateTime StartDate { get; set; }
    public int NumberOfYears { get; set; }
}