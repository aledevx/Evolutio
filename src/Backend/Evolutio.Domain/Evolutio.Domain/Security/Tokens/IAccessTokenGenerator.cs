using Evolutio.Domain.Enums;

namespace Evolutio.Domain.Security.Tokens;
public interface IAccessTokenGenerator
{
    public string Generate(Guid userIdentifier, Perfil perfil);
}

