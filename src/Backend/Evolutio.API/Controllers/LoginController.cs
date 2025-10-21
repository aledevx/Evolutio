using Evolutio.Application.UseCases.Login.DoLogin;
using Evolutio.Communication.Requests;
using Evolutio.Communication.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Evolutio.API.Controllers;
public class LoginController : EvolutioBaseController
{
    [HttpPost]
    [ProducesResponseType(typeof(ResponseRegisteredUserJson), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Login([FromBody] RequestLoginJson request, [FromServices] IDoLoginUseCase useCase)
    {
        var response = await useCase.Execute(request);

        return Ok(response);
    }
}
