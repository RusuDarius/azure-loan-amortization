using System.Net;
using System.Text.Json;
using LoanAmortization.Application.Commands.CalculateAmortization;
using LoanAmortization.Functions.Models.Requests;
using LoanAmortization.Functions.Utils;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace LoanAmortization.Functions.Functions;

public sealed class CalculateAmortizationFunction 
{
  private readonly CalculateAmortizationHandler _calculateAmortizationHandler;
  private readonly ILogger<CalculateAmortizationFunction> _logger;

  public CalculateAmortizationFunction(CalculateAmortizationHandler calculateAmortizationHandler, ILogger<CalculateAmortizationFunction> logger)
  {
    _calculateAmortizationHandler = calculateAmortizationHandler;
    _logger = logger;
  }

  [Function("CalculateAmortization")]
  public async Task<HttpResponseData> RunAsync(
    [HttpTrigger(AuthorizationLevel.Anonymous, "post",  Route = "calculate-amortization")] HttpRequestData request, FunctionContext context)
  {
    try
    {
      var requestBody = await new StreamReader(request.Body).ReadToEndAsync();
      var requestData = JsonSerializer.Deserialize<CalculateAmortizationRequest>(requestBody, ResponseFactory.JsonOptions) 
          ?? throw new InvalidOperationException("Invalid request body");

      var command = AmortizationMapper.ToCommand(requestData);

      var result = await _calculateAmortizationHandler.HandleAsync(command, context.CancellationToken);

      if(!result.IsSuccess)
      {
        return await ResponseFactory.CreateBadRequestResponse(request, result.Errors);
      }

      var paymentResponse = AmortizationMapper.ToPaymentResponse(result.Payments);
      
      return await ResponseFactory.CreateOkResponse(request, paymentResponse);
    }
    catch (Exception ex)
    {
      _logger.LogError("Error calculating amortization: {ErrorMessage}", ex.Message);

      return await ResponseFactory.CreateBadRequestResponse(request, new List<ValidationError> { new ValidationError 
        { 
          ErrorField = "Unknown", 
          ErrorMessage = "Unknown error occurred while calculating amortization." 
        }
      });
    }
  }
}