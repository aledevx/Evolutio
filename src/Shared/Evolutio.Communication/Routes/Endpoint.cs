namespace Evolutio.Communication.Routes;
public sealed record Endpoint(HttpMethod Method, string Url)
{
    public HttpMethod Method { get; init; } = Method;
    public string Url { get; init; } = Url;
}
