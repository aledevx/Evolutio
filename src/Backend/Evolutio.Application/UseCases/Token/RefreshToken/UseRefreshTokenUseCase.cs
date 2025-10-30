using Evolutio.Communication.Requests;
using Evolutio.Communication.Responses;
using Evolutio.Domain.Repositories;
using Evolutio.Domain.Repositories.Token;
using Evolutio.Domain.Security.Tokens;
using Evolutio.Exception.ExceptionsBase;
using Evolutio.Domain.ValueObjects;

namespace Evolutio.Application.UseCases.Token.RefreshToken;
public class UseRefreshTokenUseCase : IUseRefreshTokenUseCase
{
    private readonly ITokenRepository _tokenRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAccessTokenGenerator _accessTokenGenerator;
    private readonly IRefreshTokenGenerator _refreshTokenGenerator;
    public UseRefreshTokenUseCase(ITokenRepository tokenRepository,
        IUnitOfWork unitOfWork,
        IAccessTokenGenerator accessTokenGenerator,
        IRefreshTokenGenerator refreshTokenGenerator)
    {
        _tokenRepository = tokenRepository;
        _unitOfWork = unitOfWork;
        _accessTokenGenerator = accessTokenGenerator;
        _refreshTokenGenerator = refreshTokenGenerator;
    }
    public async Task<ResponseTokensJson> Execute(RequestNewToken request)
    {
        var refreshToken = await _tokenRepository.Get(request.RefreshToken);

        if (refreshToken is null) 
        {
            throw new RefreshTokenNotFoundException();
        }

        var refreshTokenValidUntil = refreshToken.CreatedOn.AddDays(EvolutioRuleConstants.REFRESH_TOKEN_EXPIRATION_DAYS);

        if (DateTime.Compare(refreshTokenValidUntil, DateTime.UtcNow) < 0)
        {
            throw new RefreshTokenExpiredException();
        }

        var newRefreshToken = new Domain.Entities.RefreshToken 
        { 
            UserId = refreshToken.UserId, 
            Value = _refreshTokenGenerator.Generate() 
        };

        await _tokenRepository.SaveNewRefreshToken(newRefreshToken);

        await _unitOfWork.CommitAsync();

        return new ResponseTokensJson
        {
            AccessToken = _accessTokenGenerator.Generate(refreshToken.User.UserIdentifier, refreshToken.User.Perfil),
            RefreshToken = newRefreshToken.Value
        };

    }
}
