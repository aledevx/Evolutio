namespace Evolutio.Communication.Routes.EvolutioApi.Health;
public static class HealthRoutes
{
    public const string DatabaseStatus = "/database-status";
    public static readonly Endpoint DatabaseStatusEndpoint = new(HttpMethod.Get, DatabaseStatus);
}
