using LoanAmortization.Application.Commands.CalculateAmortization;
using LoanAmortization.Application.Interfaces;
using LoanAmortization.Domain.Services;
using LoanAmortization.Functions.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace LoanAmortization.Functions.Program;

public class Program
{
    public static void Main()
    {
        var host = new HostBuilder()
            .ConfigureServices(services =>
            {
                services.AddApplicationInsightsTelemetryWorkerService();
                services.ConfigureFunctionsApplicationInsights();
                
                services.AddSingleton<LoanCalculator>();
                services.AddSingleton<IInterestRateProvider, FixedInterestRateProvider>();
                services.AddSingleton<CalculateAmortizationHandler>();
            })
            .Build();

        host.Run();
    }
}