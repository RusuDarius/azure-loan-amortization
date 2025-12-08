using System.Net;
using System.Text.Json;
using LoanAmortization.Application.Commands.CalculateAmortization;
using LoanAmortization.Functions.Models.Responses;
using Microsoft.Azure.Functions.Worker.Http;

namespace LoanAmortization.Functions.Utils;

internal static class ResponseFactory
{
    internal static readonly JsonSerializerOptions JsonOptions = new() { PropertyNameCaseInsensitive = true };

    internal static async Task<HttpResponseData> CreateBadRequestResponse(HttpRequestData request, List<ValidationError> errors)
    {
        var badRequestResponse = request.CreateResponse(HttpStatusCode.BadRequest);
        await badRequestResponse.WriteStringAsync(
            JsonSerializer.Serialize(
                new
                {
                    errors = errors.Select(e => new { field = e.ErrorField, message = e.ErrorMessage })
                },
                JsonOptions
            )
        );
        return badRequestResponse;
    }

    internal static async Task<HttpResponseData> CreateOkResponse(HttpRequestData request, IReadOnlyList<PaymentScheduleItemResponse> payments)
    {
        var okResponse = request.CreateResponse(HttpStatusCode.OK);
        await okResponse.WriteStringAsync(
            JsonSerializer.Serialize(
                new
                {
                    payments
                },
                JsonOptions
            )
        );
        return okResponse;
    }
}

