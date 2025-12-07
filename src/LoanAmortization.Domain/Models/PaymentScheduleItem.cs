namespace LoanAmortization.Domain.Models
{
    public class PaymentScheduleItem
    {
        public DateTime PaymentDate { get; init; }
        public decimal PaymentAmount { get; init; }
        public decimal RemainingAmount { get; init; }
    }
}
