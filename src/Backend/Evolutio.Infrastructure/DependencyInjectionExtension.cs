using Evolutio.Infrastructure.DataAccess;
using Evolutio.Infrastructure.Extensions;
using FluentMigrator.Runner;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Evolutio.Infrastructure;

public static class DependencyInjectionExtension
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        AddFluentMigrator(services, configuration);
        AddHealthChecks(services, configuration);
        AddDbContext(services, configuration);
    }
    private static void AddFluentMigrator(IServiceCollection services, IConfiguration configuration) 
    {
        var connectionString = configuration.ConnectionString();

        services.AddFluentMigratorCore().ConfigureRunner(options =>
        {
            options
                .AddSqlServer()
                .WithGlobalConnectionString(connectionString)
                .ScanIn(Assembly.Load("Evolutio.Infrastructure")).For.All();
        });
    }
    private static void AddHealthChecks(IServiceCollection services, IConfiguration configuration) 
    {
        var connectString = configuration.ConnectionString();
        services.AddHealthChecks()
            .AddSqlServer(
                connectionString: connectString,
                name: "SQL Server",
                timeout: TimeSpan.FromSeconds(5));
    }
    private static void AddDbContext(IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<EvolutioDbContext>(dbContextOptions =>
        {
            dbContextOptions.UseSqlServer(connectionString);
        });
    }
}

