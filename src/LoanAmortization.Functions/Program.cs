using Azure.Data.Tables;
using LoanAmortization.Application.Commands.CalculateAmortization;
using LoanAmortization.Application.Interfaces;
using LoanAmortization.Domain.Services;
using LoanAmortization.Infrastructure.InterestRate;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Azure.Functions.Worker;

namespace LoanAmortization.Functions.Program;

public class Program
{
    public static void Main()
    {
        var host = new HostBuilder()
            .ConfigureFunctionsWorkerDefaults()
            .ConfigureAppConfiguration((context, config) =>
            {
                config.AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                      .AddEnvironmentVariables();
            })
            .ConfigureServices((context, services) =>
            {
                services.AddApplicationInsightsTelemetryWorkerService();
                services.ConfigureFunctionsApplicationInsights();

                services.AddSingleton<LoanCalculator>();
                services.AddSingleton<IInterestRateProvider>(sp =>
                {
                    var configuration = sp.GetRequiredService<IConfiguration>();

                    var connectionString = configuration["Storage:ConnectionString"] 
                        ?? configuration["AzureWebJobsStorage"];
                    
                    var tableName = configuration["Storage:InterestRateTableName"];

                    var tableClient = new TableClient(connectionString, tableName);
                    tableClient.CreateIfNotExists();

                    return new TableInterestRateProvider(tableClient);
                });

                services.AddSingleton<CalculateAmortizationHandler>();
            })
            .Build();

        host.Run();
    }
}
