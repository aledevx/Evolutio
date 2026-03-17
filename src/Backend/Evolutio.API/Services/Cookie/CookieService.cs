using Evolutio.Domain.Services.Cookie;
using Evolutio.Communication;

namespace Evolutio.API.Services.Cookie;

public class CookieService : ICookieService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private const string AccessCookieName = Configuration.AccessTokenCookieName;
    private const string RefreshCookieName = Configuration.RefreshTokenCookieName;
    public CookieService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }
    public void ClearAuthCookies()
    {
        var response = _httpContextAccessor.HttpContext?.Response
            ?? throw new InvalidOperationException("Nenhum HttpContext ativo.");

        var expiredOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Path = "/",
            Expires = DateTime.UtcNow.AddDays(-1)
        };

        response.Cookies.Append(AccessCookieName, string.Empty, expiredOptions);
        response.Cookies.Append(RefreshCookieName, string.Empty, expiredOptions);
    }

    public void SetAuthCookies(string accessToken, string refreshToken)
    {
        var response = _httpContextAccessor.HttpContext?.Response
            ?? throw new InvalidOperationException("Nenhum HttpContext ativo.");

        var accessOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Lax,
            Expires = DateTime.UtcNow.AddMinutes(15),
            Path = "/"
        };

        var refreshOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTime.UtcNow.AddDays(7),
            Path = "/"
        };

        response.Cookies.Append(AccessCookieName, accessToken, accessOptions);
        response.Cookies.Append(RefreshCookieName, refreshToken, refreshOptions);
    }
}
