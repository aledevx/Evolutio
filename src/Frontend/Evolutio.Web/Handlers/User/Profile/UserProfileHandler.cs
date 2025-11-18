using Evolutio.Communication.Responses;
using Evolutio.Communication.Routes.EvolutioApi.User;

namespace Evolutio.Web.Handlers.User.Profile;
public class UserProfileHandler : HandlerBase, IUserProfileHandler
{
    public UserProfileHandler(HttpClient httpClient) : base(httpClient)
    {
    }

    public async Task<object> Execute()
    {
        var endpoint = UserRoutes.ProfileEndpoint;
        var result = await SendRequestWithCredentialsAsync<ResponseUserProfileJson>(endpoint.Method, endpoint.Url);

        return result;
    }
}
