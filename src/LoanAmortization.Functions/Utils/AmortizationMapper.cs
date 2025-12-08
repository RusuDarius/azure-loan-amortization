using LoanAmortization.Application.Commands.CalculateAmortization;
using LoanAmortization.Functions.Models.Requests;
using LoanAmortization.Functions.Models.Responses;

namespace LoanAmortization.Functions.Utils;

internal static class AmortizationMapper
{
    internal static CalculateAmortizationCommand ToCommand(CalculateAmortizationRequest request) =>
        new()
        {
            TotalAmount = request.TotalAmount,
            StartDate = request.StartDate,
            NumberOfYears = request.NumberOfYears,
        };

    internal static IReadOnlyList<PaymentScheduleItemResponse> ToPaymentResponse(IReadOnlyList<PaymentScheduleItemDto> payments) =>
        payments
            .Select(p => new PaymentScheduleItemResponse
            {
                PaymentDate = p.PaymentDate,
                PaymentAmount = p.PaymentAmount,
                RemainingAmount = p.RemainingAmount
            })
            .ToList();
}

