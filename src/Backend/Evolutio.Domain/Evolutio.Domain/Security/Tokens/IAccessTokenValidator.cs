namespace Evolutio.Domain.Security.Tokens;
public interface IAccessTokenValidator
{
    public (Guid identifier, string perfil) ValidateAndGetUserIdentifier(string token);
}

