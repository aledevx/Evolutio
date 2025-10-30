using Evolutio.API.Attributes;
using Evolutio.Application.UseCases.User.Delete;
using Evolutio.Application.UseCases.User.GetById;
using Evolutio.Application.UseCases.User.Profile;
using Evolutio.Application.UseCases.User.Register;
using Evolutio.Application.UseCases.User.Update;
using Evolutio.Communication.Requests;
using Evolutio.Communication.Responses;
using Evolutio.Communication.Enums;
using Microsoft.AspNetCore.Mvc;

namespace Evolutio.API.Controllers;
[AuthenticatedUser]
public class UserController : EvolutioBaseController
{
    [AuthenticatedUser(Perfil.Admin)]
    [HttpPost]
    [ProducesResponseType(typeof(ResponseRegisteredUserJson), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register([FromBody] RequestRegisterUserJson request, [FromServices] IRegisterUserUseCase useCase)
    {
        await useCase.Execute(request);

        return Created(string.Empty, new ResponseRegisteredUserJson());
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
    [AuthenticatedUser(Perfil.Admin)]
    [HttpPut]
    [Route("{id}")]
    [ProducesResponseType(typeof(ResponseUserIdJson), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update([FromRoute] long id,
        [FromBody] RequestUpdateUserJson request,
        [FromServices] IUpdateUserUseCase useCase) 
    {
        var result = await useCase.Execute(id, request);

        return Ok(result);
    }
    [HttpDelete]
    [Route("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete([FromRoute] long id, [FromServices] IDeleteUserUseCase useCase)
    {
        await useCase.Execute(id);

        return NoContent();
    }
    [HttpGet("profile")]
    [ProducesResponseType(typeof(ResponseUserProfileJson),StatusCodes.Status200OK)]
    public async Task<IActionResult> Profile([FromServices] IGetUserProfileUseCase useCase) 
    {
        var result = await useCase.Execute();
        return Ok(result);
    }
}

