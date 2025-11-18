
using Evolutio.Communication.Routes.EvolutioApi.Auth;

namespace Evolutio.Web.Handlers.Auth.Logout;
public class LogoutHandler : HandlerBase, ILogoutHandler
{
    public LogoutHandler(HttpClient httpClient) : base(httpClient)
    {
    }
    public async Task<object> Execute()
    {
        var endpoint = AuthRoutes.LogoutEndpoint;
        var result = await SendRequestWithCredentialsAsync<bool>(endpoint.Method, endpoint.Url);

        return result;
    }
}
