using Evolutio.Communication.Requests;
using Evolutio.Communication.Responses;
using Evolutio.Communication.Routes.EvolutioApi.Auth;
using Evolutio.Web.Handlers.Auth.DoLogin;

namespace Evolutio.Web.Handlers.Login.DoLogin;
public class DoLoginHandler : HandlerBase, IDoLoginHandler
{
    public DoLoginHandler(HttpClient httpClient) : base(httpClient)
    {
    }

    public async Task<object> Execute(RequestLoginJson request)
    {
        var endpoint = AuthRoutes.LoginEndpoint;
        var result = await SendRequestWithCredentialsAsync<ResponseUserNameJson>(endpoint.Method, endpoint.Url, request);

        return result;
    }
}
