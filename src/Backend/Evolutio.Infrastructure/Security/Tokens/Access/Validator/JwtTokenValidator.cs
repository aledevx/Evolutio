using Evolutio.Domain.Enums;
using Evolutio.Domain.Security.Tokens;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Evolutio.Infrastructure.Security.Tokens.Access.Validator;
public class JwtTokenValidator : JwtTokenHandler, IAccessTokenValidator
{
    private readonly string _signingKey;
    public JwtTokenValidator(string signingKey)
    {
        _signingKey = signingKey;
    }
    public (Guid identifier, Perfil perfil) ValidateAndGetUserIdentifierAndRole(string token)
    {
        var validationParameters = new TokenValidationParameters 
        {
            ValidateAudience = false,
            ValidateIssuer = false,
            IssuerSigningKey = SecurityKey(_signingKey),
            ClockSkew = TimeSpan.Zero
        };

        var tokenHandler = new JwtSecurityTokenHandler();

        var principal = tokenHandler.ValidateToken(token, validationParameters, out _);

        var userIdentifier = principal.Claims.First(c => c.Type == ClaimTypes.Sid).Value;

        var userPerfil = principal.Claims.First(c => c.Type == ClaimTypes.Role).Value;

        return (identifier: Guid.Parse(userIdentifier), perfil: Enum.Parse<Perfil>(userPerfil));

    }
}

