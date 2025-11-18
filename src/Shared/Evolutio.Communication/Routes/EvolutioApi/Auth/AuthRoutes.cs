namespace Evolutio.Communication.Routes.EvolutioApi.Auth;
public static class AuthRoutes
{
    public const string Login = "/login";
    public const string Logout = "/logout";
    public static readonly Endpoint LoginEndpoint = new(HttpMethod.Post, Login);
    public static readonly Endpoint LogoutEndpoint = new(HttpMethod.Delete, Logout);
}
