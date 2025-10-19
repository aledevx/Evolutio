using Evolutio.Domain.Security.Tokens;

namespace Evolutio.Infrastructure.Security.Tokens.Refresh;
public class RefreshTokenGenerator : IRefreshTokenGenerator
{
    public string Generate()
    {
        return Convert.ToBase64String(Guid.NewGuid().ToByteArray());
    }
}

