using Evolutio.Domain.Security.Tokens;
using Evolutio.Infrastructure.Security.Tokens.Access.Generator;

namespace CommonTestUtilities.Tokens;
public class JwtTokenGeneratorBuilder
{
    public static IAccessTokenGenerator Build() => new JwtTokenGenerator(expirationTimeMinutes: 5, signingKey: "tttttttttttttttttttttttttttttttt");
}

