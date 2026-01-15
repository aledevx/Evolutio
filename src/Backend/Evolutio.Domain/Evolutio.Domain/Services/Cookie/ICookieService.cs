
namespace Evolutio.Domain.Services.Cookie;

public interface ICookieService
{
    void SetAuthCookies(string accessToken, string refreshToken);
    void ClearAuthCookies();
}
