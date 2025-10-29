using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Repositories;
using Evolutio.Application.UseCases.Login.DoLogout;
using FluentAssertions;

namespace UseCases.Test.Login.DoLogout;
public class DoLogoutUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        (var user, _) = UserBuilder.Build();
        var useCase = CreateUseCase(user);

        Func<Task> act = async () => await useCase.Execute();

        await act.Should().NotThrowAsync();
    }
    private static DoLogoutUseCase CreateUseCase(Evolutio.Domain.Entities.User user) 
    {
        var loggedUser = LoggedUserBuilder.Build(user);
        var tokenRepository = new TokenRepositoryBuilder();
        var unitOfWork = UnitOfWorkBuilder.Build();

        return new DoLogoutUseCase(loggedUser, tokenRepository.Build(), unitOfWork);
    }
}
