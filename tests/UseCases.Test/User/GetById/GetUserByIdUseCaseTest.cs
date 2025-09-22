using CommonTestUtilities.Entities;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories;
using Evolutio.Application.UseCases.User.GetById;
using Evolutio.Exception;
using Evolutio.Exception.ExceptionsBase;
using FluentAssertions;

namespace UseCases.Test.User.GetById;
public class GetUserByIdUseCaseTest
{
    [Fact]
    public async Task Success() 
    {
        (var user, _) = UserBuilder.Build();

        var useCase = CreateUseCase(user);

        var result = await useCase.Execute(user.Id);

        result.Should().NotBeNull();
        result.Name.Should().Be(user.Name);
        result.Email.Should().Be(user.Email);
    }
    [Fact]
    public async Task Error_User_Not_Found() 
    {
        var useCase = CreateUseCase();

        Func<Task> act = async () => { await useCase.Execute(1); };

        (await act.Should().ThrowAsync<NotFoundException>())
        .Where(e => e.GetErrorMessages().Count == 1 &&
                e.GetErrorMessages().Contains(ResourceMessagesException.USER_NOT_FOUND));
    }
    private static GetUserByIdUseCase CreateUseCase(Evolutio.Domain.Entities.User user = null) 
    {
        var userReadOnlyRepository = new UserReadOnlyRepositoryBuilder();
        var mapper = MapperBuilder.Build();
        if (user is not null)
        {
            userReadOnlyRepository.GetById(user);
        }

        return new GetUserByIdUseCase(userReadOnlyRepository.Build(), mapper);
    }
}

