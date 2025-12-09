using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Azure.Functions.Worker;
using LoanAmortization.Functions.Extensions;
using LoanAmortization.Infrastructure.Seeding;

namespace LoanAmortization.Functions.Program;

public class Program
{
    public static async Task Main()
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

                services.AddApplicationServices();
                services.AddInfrastructureServices(context.Configuration);
            })
            .Build();

        await host.Services.GetRequiredService<IStorageSeeder>().SeedAsync();
        host.Run();
    }
}
