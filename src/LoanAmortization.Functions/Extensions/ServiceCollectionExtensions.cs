using Azure.Data.Tables;
using LoanAmortization.Application.Commands.CalculateAmortization;
using LoanAmortization.Application.Interfaces;
using LoanAmortization.Domain.Services;
using LoanAmortization.Infrastructure.InterestRate;
using LoanAmortization.Infrastructure.Seeding;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LoanAmortization.Functions.Extensions;

public static class ServiceCollectionExtensions
{
  public static IServiceCollection AddApplicationServices(this IServiceCollection services)
  {
    services.AddSingleton<LoanCalculator>();
    services.AddSingleton<CalculateAmortizationHandler>();

    return services;
  }

  public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
  {
    var connectionString = configuration["Storage:ConnectionString"] ?? configuration["AzureWebJobsStorage"];
    var tableName = configuration["Storage:InterestRateTableName"];
    var tableClient = new TableClient(connectionString, tableName);
    tableClient.CreateIfNotExists();

    services.AddSingleton<IInterestRateProvider>(new TableInterestRateProvider(tableClient));
    services.AddSingleton<IStorageSeeder>(sp => new StorageSeeder(tableClient));

    return services;
  }
}
