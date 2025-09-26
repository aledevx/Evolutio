using Azure.Core;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using Evolutio.Application.UseCases.User.Delete;
using Evolutio.Exception;
using Evolutio.Exception.ExceptionsBase;
using FluentAssertions;

namespace UseCases.Test.User.Delete;
public class DeleteUserUseCaseTest
{
    [Fact]
    public async Task Success() 
    {
        (var user,_) = UserBuilder.Build();

        var useCase = CreateUseCase(user.Id);

        Func<Task> act = async () => { await useCase.Execute(user.Id); };

        await act.Should().NotThrowAsync();
    }
    [Fact]
    public async Task Error_User_Not_Found()
    {
        (var user, _) = UserBuilder.Build();

        var useCase = CreateUseCase(user.Id);

        Func<Task> act = async () => await useCase.Execute(2);

        (await act.Should().ThrowAsync<NotFoundException>())
               .Where(e => e.GetErrorMessages().Count == 1 &&
                           e.GetErrorMessages().Contains(ResourceMessagesException.USER_NOT_FOUND));
    }
    private static DeleteUserUseCase CreateUseCase(long? id) 
    {
        var deleteUserRepository = UserDeleteOnlyRepositoryBuilder.Build();
        var readUserRepository = new UserReadOnlyRepositoryBuilder();
        var unitOfWork = UnitOfWorkBuilder.Build();
        if (id.HasValue) 
        {
            readUserRepository.ExistsById(id.Value);
        }

        return new DeleteUserUseCase(deleteUserRepository, readUserRepository.Build(), unitOfWork);
    }
}

