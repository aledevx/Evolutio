using Evolutio.API.Attributes;
using Evolutio.Application.UseCases.Login.DoLogin;
using Evolutio.Application.UseCases.Login.DoLogout;
using Evolutio.Communication.Requests;
using Evolutio.Communication.Responses;
using Evolutio.Communication.Routes.EvolutioApi.Auth;
using Microsoft.AspNetCore.Mvc;

namespace Evolutio.API.Controllers;
public class AuthController : EvolutioBaseController
{
    [HttpPost]
    [Route(AuthRoutes.Login)]
    [ProducesResponseType(typeof(ResponseUserNameJson), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Login([FromBody] RequestLoginJson request, [FromServices] IDoLoginUseCase useCase)
    {
        var response = await useCase.Execute(request);

        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true, // em dev com HTTPS. Se não tiver HTTPS no dev, usar SecurePolicy/Careful.
            SameSite = SameSiteMode.None, // ou Strict dependendo do fluxo
            Expires = DateTime.UtcNow.AddMinutes(1000),
            Path = "/"
        };

        Response.Cookies.Append("access_token", response.Tokens.AccessToken, cookieOptions);
        cookieOptions.Expires = DateTime.UtcNow.AddDays(7);
        Response.Cookies.Append("refresh_token", response.Tokens.RefreshToken, cookieOptions);

        return Ok(new ResponseUserNameJson(response.Name));
    }
    [HttpDelete]
    [Route(AuthRoutes.Logout)]
    [AuthenticatedUser]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Logout([FromServices] IDoLogoutUseCase useCase)
    {
        await useCase.Execute();

        // Remove cookie
        Response.Cookies.Delete("access_token");
        Response.Cookies.Delete("refresh_token");

        return NoContent();
    }
}
