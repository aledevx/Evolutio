using Evolutio.API.Attributes;
using Evolutio.Communication.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Evolutio.API.Controllers;

public class HealthController : EvolutioBaseController
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
    [AuthenticatedUser(Perfil.Admin)]
    [HttpGet("only-admin")]
    public IActionResult TestandoRoleFuncionario()
    {
        return Ok("Tudo ok até aqui");
    }
    [AuthenticatedUser(Perfil.Funcionario)]
    [HttpGet("only-funcionario")]
    public IActionResult TestandoRoleAdmin()
    {
        return Ok("Tudo ok até aqui");
    }
    [AuthenticatedUser(Perfil.Admin, Perfil.Funcionario)]
    [HttpGet("admin-and-funcionario")]
    public IActionResult TestandoRoleParaAmbos()
    {
        return Ok("Tudo ok até aqui");
    }
}

