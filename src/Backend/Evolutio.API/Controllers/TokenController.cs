using Evolutio.Application.UseCases.Token.RefreshToken;
using Evolutio.Communication.Requests;
using Evolutio.Communication.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Evolutio.API.Controllers;
public class TokenController : EvolutioBaseController
{
    [HttpPost("refresh-token")]
    [ProducesResponseType(typeof(ResponseTokensJson), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RefreshToken([FromBody] RequestNewToken request, [FromServices] IUseRefreshTokenUseCase useCase)
    {
        var result = await useCase.Execute(request);

        return Created(string.Empty, result);
    }
}