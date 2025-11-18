using Evolutio.Communication.Responses;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Evolutio.Application.UseCases.Health.DatabaseStatus;
public class DatabaseStatusUseCase : IDatabaseStatusUseCase
{
    private readonly HealthCheckService _healthCheckService;
    public DatabaseStatusUseCase(HealthCheckService healthCheckService)
    {
        _healthCheckService = healthCheckService;
    }
    public async Task<ResponseDatabaseStatusJson> Execute()
    {
        var report = await _healthCheckService.CheckHealthAsync();

        var result = new ResponseDatabaseStatusJson
        {
            Status = report.Status.ToString(),
            Checks = report.Entries.Select(e => new Communication.Responses.ResponseCheckJson
            {
                Name = e.Key,
                Status = e.Value.Status.ToString(),
                Description = e.Value.Description
            }).ToList(),
            TotalDuration = report.TotalDuration.TotalMilliseconds
        };

        return result;
    }
}
