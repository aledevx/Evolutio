using Evolutio.Domain.Security.Tokens;

namespace Evolutio.API.Token;
public class HttpContextTokenValue : ITokenProvider
{
    private readonly IHttpContextAccessor _contextAccessor;
    public HttpContextTokenValue(IHttpContextAccessor contextAccessor)
    {
        _contextAccessor = contextAccessor;
    }
    public string Value()
    {
        var accessToken = _contextAccessor.HttpContext!.Request.Cookies.TryGetValue("access_token", out var token);

        return token!;
    }
}
