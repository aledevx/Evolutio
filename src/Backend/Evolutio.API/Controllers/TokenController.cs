using Evolutio.Application.UseCases.Token.RefreshToken;
using Evolutio.Communication.Requests;
using Evolutio.Communication.Responses;
using Evolutio.Communication.Routes.EvolutioApi.Token;
using Evolutio.Domain.Services.Cookie;
using Microsoft.AspNetCore.Mvc;

namespace Evolutio.API.Controllers;
public class TokenController : EvolutioBaseController
{
    [HttpPost(TokenRoutes.RefreshToken)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RefreshToken(
        [FromBody] RequestNewToken request,
        [FromServices] IUseRefreshTokenUseCase useCase,
        [FromServices] ICookieService cookieService)
    {
        var result = await useCase.Execute(request);

        // Remove cookies antigos via serviço
        cookieService.ClearAuthCookies();

        // Atualiza access token e fefre
        cookieService.SetAuthCookies(result.AccessToken, result.RefreshToken);

        return Ok();
    }
}