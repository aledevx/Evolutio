using Evolutio.Communication.Responses;
using Evolutio.Communication.Routes.EvolutioApi.Token;

namespace Evolutio.Web.Handlers.Token.RefreshToken;
public class RefreshTokenHandler : HandlerBase, IRefreshTokenHandler
{
    public RefreshTokenHandler(HttpClient httpClient) : base(httpClient)
    {
    }
    public async Task<object> Execute()
    {
        var endpoint = TokenRoutes.RefreshTokenEndpoint;
        var result = await SendRequestWithCredentialsAsync<bool>(endpoint.Method, endpoint.Url);
        return result;
    }
}
