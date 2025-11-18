using Evolutio.Communication.Responses;
using Evolutio.Communication.Routes.EvolutioApi.Health;

namespace Evolutio.Web.Handlers.Health.DatabaseStatus;
public class DatabaseStatusHandler : HandlerBase, IDatabaseStatusHandler
{
    public DatabaseStatusHandler(HttpClient httpClient) : base(httpClient)
    {
    }

    public async Task<object> Execute() 
    {
        var endpoint = HealthRoutes.DatabaseStatusEndpoint;
        var result = await SendRequestWithCredentialsAsync<ResponseDatabaseStatusJson>(endpoint.Method, endpoint.Url);

        return result;
    }
}
