using AutoMapper;
using Evolutio.Communication.Requests;
using Evolutio.Communication.Responses;
using Evolutio.Domain.Repositories;
using Evolutio.Domain.Repositories.Token;
using Evolutio.Domain.Repositories.User;
using Evolutio.Domain.Security.Cryptography;
using Evolutio.Domain.Security.Tokens;
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
    private readonly ITokenRepository _tokenRepository;
    private readonly IAccessTokenGenerator _accessTokenGenerator;
    private readonly IRefreshTokenGenerator _refreshTokenGenerator;
    public RegisterUserUseCase(IUserWriteOnlyRepository writeOnlyRepository,
        IUserReadOnlyRepository readOnlyRepository,
        IUnitOfWork unitOfWork,
        IPasswordEncripter passwordEncripter,
        IMapper mapper,
        ITokenRepository tokenRepository,
        IAccessTokenGenerator accessTokenGenerator,
        IRefreshTokenGenerator refreshTokenGenerator)
    {
        _writeOnlyRepository = writeOnlyRepository;
        _readOnlyRepository = readOnlyRepository;
        _unitOfWork = unitOfWork;
        _passwordEncripter = passwordEncripter;
        _mapper = mapper;
        _tokenRepository = tokenRepository;
        _accessTokenGenerator = accessTokenGenerator;
        _refreshTokenGenerator = refreshTokenGenerator;
    }
    public async Task<ResponseRegisteredUserJson> Execute(RequestRegisterUserJson request)
    {
        await Validate(request);

        var user = _mapper.Map<Domain.Entities.User>(request);
        user.Password = _passwordEncripter.Encrypt(request.Password);

        await _writeOnlyRepository.Add(user);
        await _unitOfWork.CommitAsync();

        /// TODO: CRIAR E RETORNAR OS TOKENS JWT
        /// 

        var tokens = new ResponseTokensJson 
        {
            AccessToken = _accessTokenGenerator.Generate(user.UserIdentifier, user.Perfil),
            RefreshToken = await CreateAndSaveRefreshToken(user)
        };

        var result = new ResponseRegisteredUserJson(user.Name, tokens);

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

