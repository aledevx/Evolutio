namespace Evolutio.Communication.Routes.EvolutioApi.Token;
public static class TokenRoutes
{
    public const string RefreshToken = "/refresh-token";
    public static readonly Endpoint RefreshTokenEndpoint = new(HttpMethod.Post, RefreshToken);
}
