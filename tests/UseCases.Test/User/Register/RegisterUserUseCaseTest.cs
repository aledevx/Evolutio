using CommonTestUtilities.Cryptography;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Tokens;
using Evolutio.Application.UseCases.User.Register;
using Evolutio.Exception;
using Evolutio.Exception.ExceptionsBase;
using FluentAssertions;

namespace UseCases.Test.User.Register;
public class RegisterUserUseCaseTest
{
    [Fact]
    public async Task Success() 
    {
        var request = RequestRegisterUserJsonBuilder.Build();

        var useCase = CreateUseCase();

        Func<Task> act = async () => await useCase.Execute(request); await useCase.Execute(request);

        await act.Should().NotThrowAsync();
    }
    [Fact]
    public async Task Error_Email_Already_Exists() 
    {
        var request = RequestRegisterUserJsonBuilder.Build();

        var useCase = CreateUseCase(request.Email);

        Func<Task> act = async () => await useCase.Execute(request);

        (await act.Should()
            .ThrowAsync<ErrorOnValidationException>())
            .Where(e => e.GetErrorMessages().Count == 1 && 
            e.GetErrorMessages().Contains(ResourceMessagesException.EMAIL_ALREADY_REGISTERED));
    }
    private static RegisterUserUseCase CreateUseCase(string? email = null) 
    {
        var writeOnlyRepository = UserWriteOnlyRepositoryBuilder.Build();
        var readOnlyRepository = new UserReadOnlyRepositoryBuilder();
        var unitOfWork = UnitOfWorkBuilder.Build();
        var passwordEncripter = PasswordEncripterBuilder.Build();
        var mapper = MapperBuilder.Build();

        if (String.IsNullOrWhiteSpace(email) == false) 
        {
            readOnlyRepository.ExistsByEmail(email);
        }

        return new RegisterUserUseCase(writeOnlyRepository, 
            readOnlyRepository.Build(), 
            unitOfWork, 
            passwordEncripter, 
            mapper);
    }
}

