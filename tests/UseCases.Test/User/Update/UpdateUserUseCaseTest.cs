using CommonTestUtilities.Entities;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using Evolutio.Application.UseCases.User.Update;
using Evolutio.Exception;
using Evolutio.Exception.ExceptionsBase;
using FluentAssertions;

namespace UseCases.Test.User.Update;
public class UpdateUserUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        (var user, _) = UserBuilder.Build();
        var request = RequestUpdateUserJsonBuilder.Build();

        var useCase = CreateUseCase(user);

        var act = async () => await useCase.Execute(user.Id, request);

        await act.Should().NotThrowAsync();

        user.Name.Should().Be(request.Name);
        user.Email.Should().Be(request.Email);
    }
    [Fact]
    public async Task Error_User_Not_Found()
    {
        var useCase = CreateUseCase();

        var request = RequestUpdateUserJsonBuilder.Build();

        Func<Task> act = async () => await useCase.Execute(1, request);

        (await act.Should().ThrowAsync<NotFoundException>())
           .Where(e => e.GetErrorMessages().Count == 1 &&
                       e.GetErrorMessages().Contains(ResourceMessagesException.USER_NOT_FOUND));
    }
    [Fact]
    public async Task Error_Email_Already_Registered()
    {
        (var user, _) = UserBuilder.Build();

        var request = RequestUpdateUserJsonBuilder.Build();

        var useCase = CreateUseCase(user, request.Email);

        Func<Task> act = async () => await useCase.Execute(user.Id, request);

        (await act.Should().ThrowAsync<ErrorOnValidationException>())
           .Where(e => e.GetErrorMessages().Count == 1 &&
                       e.GetErrorMessages().Contains(ResourceMessagesException.EMAIL_ALREADY_REGISTERED));

        user.Name.Should().NotBe(request.Name);
        user.Email.Should().NotBe(request.Email);
    }
    private static UpdateUserUseCase CreateUseCase(Evolutio.Domain.Entities.User? user = null, string? email = null)
    {
        var readOnlyRepository = new UserReadOnlyRepositoryBuilder();
        var updateOnlyRepository = new UserUpdateOnlyRepositoryBuilder();   
        var mapper = MapperBuilder.Build();
        var unitOfWork = UnitOfWorkBuilder.Build();

        if (user is not null)
        {
            readOnlyRepository.GetByUserIdentifier(user);
            updateOnlyRepository.GetById(user);
        
            if ( email is not null) 
            {
                readOnlyRepository.ExistsByEmail(email);
            }
        }

            return new UpdateUserUseCase(readOnlyRepository.Build(), updateOnlyRepository.Build(), mapper, unitOfWork);
    }
}

