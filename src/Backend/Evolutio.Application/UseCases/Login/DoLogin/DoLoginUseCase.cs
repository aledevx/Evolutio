
using Evolutio.Communication;
using Evolutio.Communication.Requests;
using Evolutio.Communication.Responses;
using Evolutio.Domain.Repositories;
using Evolutio.Domain.Repositories.Token;
using Evolutio.Domain.Repositories.User;
using Evolutio.Domain.Security.Cryptography;
using Evolutio.Domain.Security.Tokens;
using Evolutio.Exception.ExceptionsBase;

namespace Evolutio.Application.UseCases.Login.DoLogin;
public class DoLoginUseCase : IDoLoginUseCase
{
    private readonly IUserReadOnlyRepository _userReadOnlyRepository;
    private readonly IPasswordEncripter _passwordEncripter;
    private readonly ITokenRepository _tokenRepository;
    private readonly IAccessTokenGenerator _accessTokenGenerator;
    private readonly IRefreshTokenGenerator _refreshTokenGenerator;
    private readonly IUnitOfWork _unitOfWork;
    public DoLoginUseCase(IPasswordEncripter passwordEncripter,
        ITokenRepository tokenRepository,
        IAccessTokenGenerator accessTokenGenerator,
        IRefreshTokenGenerator refreshTokenGenerator,
        IUserReadOnlyRepository userReadOnlyRepository,
        IUnitOfWork unitOfWork)
    {
        _passwordEncripter = passwordEncripter;
        _tokenRepository = tokenRepository;
        _accessTokenGenerator = accessTokenGenerator;
        _refreshTokenGenerator = refreshTokenGenerator;
        _userReadOnlyRepository = userReadOnlyRepository;
        _unitOfWork = unitOfWork;
    }
    public async Task<ResponseLoggedUserJson> Execute(RequestLoginJson request)
    {
        var user = await _userReadOnlyRepository.GetByEmail(request.Email);

        if (user is null || _passwordEncripter.IsValid(request.Password, user.Password) is false) 
        {
            throw new InvalidLoginException();
        }

        var tokens = new ResponseTokensJson() 
        {
            AccessToken = _accessTokenGenerator.Generate(user.UserIdentifier, user.Perfil),
            RefreshToken = await CreateAndSaveRefreshToken(user)
        };
      
        return new ResponseLoggedUserJson(user.Name, tokens);
    }
    private async Task<string> CreateAndSaveRefreshToken(Domain.Entities.User user)
    {
        var refreshToken = new Domain.Entities.RefreshToken
        {
            UserId = user.Id,
            Value = _refreshTokenGenerator.Generate()
        };

        await _tokenRepository.SaveNewRefreshToken(refreshToken);

        await _unitOfWork.CommitAsync();

        return refreshToken.Value;
    }
}
