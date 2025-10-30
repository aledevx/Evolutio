using Evolutio.Domain.Repositories;
using Evolutio.Domain.Repositories.Token;
using Evolutio.Domain.Services.LoggedUser;

namespace Evolutio.Application.UseCases.Login.DoLogout;
public class DoLogoutUseCase : IDoLogoutUseCase
{
    private readonly ILoggedUser _loggedUser;
    private readonly ITokenRepository _tokenRepository;
    private readonly IUnitOfWork _unitOfWork;
    public DoLogoutUseCase(ILoggedUser loggedUser,
        ITokenRepository tokenRepository,
        IUnitOfWork unitOfWork)
    {
        _loggedUser = loggedUser;
        _tokenRepository = tokenRepository;
        _unitOfWork = unitOfWork;
    }
    public async Task Execute()
    {
        var user = await _loggedUser.User();

        _tokenRepository.RemoveAllRefreshTokens(user.Id);

        await _unitOfWork.CommitAsync();
    }
}
