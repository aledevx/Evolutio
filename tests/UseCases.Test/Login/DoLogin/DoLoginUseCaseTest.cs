using CommonTestUtilities.Cryptography;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Tokens;
using Evolutio.Application.UseCases.Login.DoLogin;
using Evolutio.Communication.Requests;
using Evolutio.Exception;
using Evolutio.Exception.ExceptionsBase;
using FluentAssertions;

namespace UseCases.Test.Login.DoLogin;
public class DoLoginUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        (var user, var password) = UserBuilder.Build();

        var useCase = CreateUseCase(user);

        var request = new RequestLoginJson { Email = user.Email, Password = password };

        var result = await useCase.Execute(request);

        result.Should().NotBeNull();
        result.Tokens.Should().NotBeNull();
        result.Name.Should().NotBeNullOrWhiteSpace().And.Be(user.Name);
        result.Tokens.AccessToken.Should().NotBeNullOrEmpty();
        result.Tokens.RefreshToken.Should().NotBeNullOrEmpty();
    }
    [Fact]
    public async Task Error_Invalid_User()
    {
        (var user, _) = UserBuilder.Build();

        var useCase = CreateUseCase(user);

        var request = RequestLoginJsonBuilder.Build();

        Func<Task> act = async () => await useCase.Execute(request);

        (await act.Should()
            .ThrowAsync<InvalidLoginException>())
            .Where(e => e.GetErrorMessages().Count == 1 &&
            e.GetErrorMessages().Contains(ResourceMessagesException.EMAIL_OR_PASSWORD_INVALID));
    }
    private static DoLoginUseCase CreateUseCase(Evolutio.Domain.Entities.User? user = null) 
    {
        var userReadOnlyRepository = new UserReadOnlyRepositoryBuilder();
        var passwordEncripter = PasswordEncripterBuilder.Build();
        var tokenRepository = new TokenRepositoryBuilder();
        var accessTokenGenerator = JwtTokenGeneratorBuilder.Build();
        var refreshTokenGenerator = RefreshTokenGeneratorBuilder.Build();
        var unitOfWork = UnitOfWorkBuilder.Build();

        if (user is not null) 
        {
            userReadOnlyRepository.GetByEmail(user);
        }

        return new DoLoginUseCase(passwordEncripter,
            tokenRepository.Build(),
            accessTokenGenerator,
            refreshTokenGenerator,
            userReadOnlyRepository.Build(),
            unitOfWork);
    }
}
