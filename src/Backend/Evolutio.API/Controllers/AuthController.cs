using Evolutio.API.Attributes;
using Evolutio.Application.UseCases.Login.DoLogin;
using Evolutio.Application.UseCases.Login.DoLogout;
using Evolutio.Communication.Requests;
using Evolutio.Communication.Responses;
using Evolutio.Communication.Routes.EvolutioApi.Auth;
using Evolutio.Domain.Services.Cookie;
using Microsoft.AspNetCore.Mvc;

namespace Evolutio.API.Controllers;
public class AuthController : EvolutioBaseController
{
    [HttpPost]
    [Route(AuthRoutes.Login)]
    [ProducesResponseType(typeof(ResponseUserNameJson), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Login([FromBody] RequestLoginJson request, [FromServices] IDoLoginUseCase useCase, ICookieService cookieService)
    {
        var response = await useCase.Execute(request);
        cookieService.SetAuthCookies(response.Tokens.AccessToken, response.Tokens.RefreshToken);
        return Ok(new ResponseUserNameJson(response.Name));
    }
    [HttpDelete]
    [Route(AuthRoutes.Logout)]
    [AuthenticatedUser]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Logout([FromServices] IDoLogoutUseCase useCase, ICookieService cookieService)
    {
        await useCase.Execute();
        cookieService.ClearAuthCookies();
        return NoContent();
    }
}
