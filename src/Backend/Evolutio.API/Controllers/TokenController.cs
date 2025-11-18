using Azure;
using Evolutio.Application.UseCases.Token.RefreshToken;
using Evolutio.Communication.Requests;
using Evolutio.Communication.Responses;
using Evolutio.Communication.Routes.EvolutioApi.Token;
using Microsoft.AspNetCore.Mvc;

namespace Evolutio.API.Controllers;
public class TokenController : EvolutioBaseController
{
    [HttpPost(TokenRoutes.RefreshToken)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RefreshToken([FromBody] RequestNewToken request, [FromServices] IUseRefreshTokenUseCase useCase)
    {
        var result = await useCase.Execute(request);

        // Remove cookie
        Response.Cookies.Delete("access_token");
        Response.Cookies.Delete("refresh_token");

        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true, // em dev com HTTPS. Se não tiver HTTPS no dev, usar SecurePolicy/Careful.
            SameSite = SameSiteMode.None, // ou Strict dependendo do fluxo
            Expires = DateTime.UtcNow.AddMinutes(1000),
            Path = "/"
        };

        Response.Cookies.Append("access_token", result.AccessToken, cookieOptions);
        cookieOptions.Expires = DateTime.UtcNow.AddDays(7);
        Response.Cookies.Append("refresh_token", result.RefreshToken, cookieOptions);

        return Ok();
    }
}