using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Mapper;
using Evolutio.Application.UseCases.User.Profile;
using FluentAssertions;

namespace UseCases.Test.User.Profile;
public class GetProfileUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        (var user, _) = UserBuilder.Build();
        var useCase = CreateUseCase(user);

        var result = await useCase.Execute();

        result.Should().NotBeNull();
        result.Name.Should().Be(user.Name);
        result.Email.Should().Be(user.Email);
        result.Active.Should().Be(user.Active);
    }
    private static GetUserProfileUseCase CreateUseCase(Evolutio.Domain.Entities.User user)
    {
        var loggedUser = LoggedUserBuilder.Build(user);
        var mapper = MapperBuilder.Build();

        return new GetUserProfileUseCase(loggedUser, mapper);
    }
}
