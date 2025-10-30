using Evolutio.Domain.Enums;

namespace Evolutio.Domain.Security.Tokens;
public interface IAccessTokenValidator
{
    public (Guid identifier, Perfil perfil) ValidateAndGetUserIdentifierAndRole(string token);
}

