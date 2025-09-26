using AutoMapper;
using Evolutio.Communication.Requests;
using Evolutio.Communication.Responses;
using Evolutio.Domain.Repositories;
using Evolutio.Domain.Repositories.User;
using Evolutio.Exception;
using Evolutio.Exception.ExceptionsBase;

namespace Evolutio.Application.UseCases.User.Update;
public class UpdateUserUseCase : IUpdateUserUseCase
{
    private readonly IUserReadOnlyRepository _readOnlyRepository;
    private readonly IUserUpdateOnlyRepository _updateOnlyRepository;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    public UpdateUserUseCase(IUserReadOnlyRepository readOnlyRepository,
        IUserUpdateOnlyRepository updateOnlyRepository,
        IMapper mapper,
        IUnitOfWork unitOfWork)
    {
        _readOnlyRepository = readOnlyRepository;
        _updateOnlyRepository = updateOnlyRepository;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }
    public async Task<ResponseUserIdJson> Execute(long id, RequestUpdateUserJson request)
    {
        await Validate(id, request);

        var user = await _updateOnlyRepository.GetById(id);

        if (user is null) 
        {
            throw new NotFoundException(ResourceMessagesException.USER_NOT_FOUND);
        }

        _mapper.Map(request, user);
        
        _updateOnlyRepository.Update(user);

        await _unitOfWork.CommitAsync();

        return new ResponseUserIdJson(user.Id);
    }
    private async Task Validate(long id,RequestUpdateUserJson request) 
    {
        var validator = new UpdateUserValidator();

        var result = validator.Validate(request);

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

