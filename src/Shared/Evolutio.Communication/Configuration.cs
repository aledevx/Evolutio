namespace Evolutio.Communication;
public class Configuration
{
    public const string AccessTokenCookieName = "__Host-access_token";
    public const string RefreshTokenCookieName = "__Host-refresh_token";
    public static string CorsPolicyName = "wasm";
    public const string HttpClientName = "EvolutioAPI";
    public static string BackendUrl { get; set; } = "https://localhost:7001";
    public static string FrontendUrl { get; set; } = "https://localhost:7002";
}
