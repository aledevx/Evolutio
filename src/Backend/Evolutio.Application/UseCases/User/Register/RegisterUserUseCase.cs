using AutoMapper;
using Evolutio.Communication.Requests;
using Evolutio.Communication.Responses;
using Evolutio.Domain.Repositories;
using Evolutio.Domain.Repositories.User;
using Evolutio.Domain.Security.Cryptography;
using Evolutio.Exception;
using Evolutio.Exception.ExceptionsBase;

namespace Evolutio.Application.UseCases.User.Register;
public class RegisterUserUseCase : IRegisterUserUseCase
{
    private readonly IUserWriteOnlyRepository _writeOnlyRepository;
    private readonly IUserReadOnlyRepository _readOnlyRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordEncripter _passwordEncripter;
    private readonly IMapper _mapper;
    public RegisterUserUseCase(IUserWriteOnlyRepository writeOnlyRepository,
        IUserReadOnlyRepository readOnlyRepository,
        IUnitOfWork unitOfWork,
        IPasswordEncripter passwordEncripter,
        IMapper mapper)
    {
        _writeOnlyRepository = writeOnlyRepository;
        _readOnlyRepository = readOnlyRepository;
        _unitOfWork = unitOfWork;
        _passwordEncripter = passwordEncripter;
        _mapper = mapper;
    }
    public async Task<ResponseRegisteredUserJson> Execute(RequestRegisterUserJson request)
    {
        await Validate(request);

        ///TODO: MAPEAR O USUARIO
        var user = _mapper.Map<Domain.Entities.User>(request);
        ///TODO: CRIPTOGRAFAR A SENHA COM BCRYPT
        user.Password = _passwordEncripter.Encrypt(request.Password);

        await _writeOnlyRepository.Add(user);
        await _unitOfWork.CommitAsync();

        var result = new ResponseRegisteredUserJson(user.Name);

        return result;

    }
    private async Task Validate(RequestRegisterUserJson request)
    {
        var validator = new RegisterUserValidator();
        var result = await validator.ValidateAsync(request);

        var emailExists = await _readOnlyRepository.ExistsByEmail(request.Email);

        if (emailExists) 
        {
            result.Errors.Add(new FluentValidation.Results.ValidationFailure(string.Empty, ResourceMessagesException.EMAIL_ALREADY_REGISTERED));
        }

        if (result.Errors.Any()) 
        {
            var errorsMessages = result.Errors.Select(e => e.ErrorMessage).ToList();

            throw new ErrorOnValidationException(errorsMessages);
        }

    }
}

