using Evolutio.Communication.Requests;
using Evolutio.Communication.Responses;

namespace Evolutio.Application.UseCases.Token.RefreshToken;
public interface IUseRefreshTokenUseCase
{
    public Task<ResponseTokensJson> Execute(RequestNewToken request);
}

