using Evolutio.Domain.Entities;

namespace Evolutio.Domain.Repositories.Token;
public interface ITokenRepository
{
    public Task<RefreshToken?> Get(string refreshToken);
    public Task SaveNewRefreshToken(RefreshToken refreshToken);
    public void RemoveAllRefreshTokens(long userId);
}

