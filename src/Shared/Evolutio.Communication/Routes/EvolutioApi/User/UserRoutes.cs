namespace Evolutio.Communication.Routes.EvolutioApi.User;
public static class UserRoutes
{
    public const string Register = "/user";
    public const string GetById = "/user";
    public const string Update = "/user";
    public const string Delete = "/user";
    public const string Profile = "/user/profile";
    public static readonly Endpoint RegisterEndpoint = new(HttpMethod.Post, Register);
    public static readonly Endpoint GetByIdEndpoint = new(HttpMethod.Get, GetById);
    public static readonly Endpoint UpdateEndpoint = new(HttpMethod.Put, Update);
    public static readonly Endpoint DeleteEndpoint = new(HttpMethod.Delete, Delete);
    public static readonly Endpoint ProfileEndpoint = new(HttpMethod.Get, Profile);

}
