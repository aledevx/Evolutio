using Evolutio.Application.UseCases.User.GetById;
using Evolutio.Application.UseCases.User.Register;
using Evolutio.Communication.Requests;
using Evolutio.Communication.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Evolutio.API.Controllers;

public class UserController : EvolutioBaseController
{
    [HttpPost]
    [ProducesResponseType(typeof(ResponseRegisteredUserJson), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register([FromBody] RequestRegisterUserJson request, [FromServices] IRegisterUserUseCase useCase)
    {
        var result = await useCase.Execute(request);

        return Created(string.Empty, result);
    }
    [HttpGet]
    [Route("{id}")]
    [ProducesResponseType(typeof(ResponseUserProfileJson), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById([FromRoute] long id, [FromServices] IGetUserByIdUseCase useCase) 
    {
        var result = await useCase.Execute(id);

        return Ok(result);
    }    
}

