using Evolutio.Domain.Security.Tokens;
using Evolutio.Infrastructure.Security.Tokens.Refresh;

namespace CommonTestUtilities.Tokens;
public class RefreshTokenGeneratorBuilder
{
    public static IRefreshTokenGenerator Build() => new RefreshTokenGenerator();
}
