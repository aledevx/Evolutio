using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Evolutio.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class HealthController : ControllerBase
{

    [HttpGet]
    public async Task<IActionResult> Get([FromServices] HealthCheckService healthCheckService)
    {
        var report = await healthCheckService.CheckHealthAsync();

        var result = new
        {
            status = report.Status.ToString(),
            checks = report.Entries.Select(e => new
            {
                name = e.Key,
                status = e.Value.Status.ToString(),
                description = e.Value.Description
            }),
            totalDuration = report.TotalDuration.TotalMilliseconds
        };

        return Ok(result);
    }
    [HttpGet("test")]
    public IActionResult Funfando()
    {
        return Ok("Tudo ok até aqui");
    }
}

