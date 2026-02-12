using Evolutio.Communication;
using Evolutio.Domain.Security.Tokens;

namespace Evolutio.API.Token;
public class HttpContextTokenValue : ITokenProvider
{
    private readonly IHttpContextAccessor _contextAccessor;
    private const string AccessTokenCookie = Configuration.AccessTokenCookieName;
    public HttpContextTokenValue(IHttpContextAccessor contextAccessor)
    {
        _contextAccessor = contextAccessor;
    }
    public string Value()
    {
        var accessToken = _contextAccessor.HttpContext!.Request.Cookies.TryGetValue(AccessTokenCookie, out var token);

        return token!;
    }
}
