namespace LoanAmortization.Functions.Models.Requests;

/// <summary>
/// Sample request body:
/// {
///  "TotalSum": 100000,
///  "StartDate": "2025-01-15",
///  "NumberOfYears": 10
/// }
/// </summary>

public sealed class CalculateAmortizationRequest 
{
    public decimal TotalAmount { get; set; }
    public DateTime StartDate { get; set; }
    public int NumberOfYears { get; set; }
}