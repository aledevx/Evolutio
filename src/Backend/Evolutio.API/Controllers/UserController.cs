using Evolutio.Application.UseCases.User.Register;
using Evolutio.Communication.Requests;
using Evolutio.Communication.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Evolutio.API.Controllers;

public class UserController : EvolutioBaseController
{
    [HttpPost]
    [ProducesResponseType(typeof(ResponseRegisteredUserJson), StatusCodes.Status201Created)]
    public async Task<IActionResult> Register([FromBody] RequestRegisterUserJson request, [FromServices] IRegisterUserUseCase useCase)
    {
        var result = await useCase.Execute(request);

        return Created(string.Empty, result);
    }
}

