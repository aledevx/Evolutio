using Evolutio.API.Attributes;
using Evolutio.Application.UseCases.Health.DatabaseStatus;
using Evolutio.Communication.Enums;
using Evolutio.Communication.Routes.EvolutioApi.Health;
using Microsoft.AspNetCore.Mvc;

namespace Evolutio.API.Controllers;

public class HealthController : EvolutioBaseController
{
    [AuthenticatedUser(Perfil.Admin)]
    [HttpGet(HealthRoutes.DatabaseStatus)]
    public async Task<IActionResult> DatabaseStatus([FromServices] IDatabaseStatusUseCase useCase)
    {
        var result = await useCase.Execute();

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

