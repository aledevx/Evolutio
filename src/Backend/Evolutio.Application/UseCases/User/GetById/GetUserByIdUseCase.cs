using AutoMapper;
using Evolutio.Communication.Responses;
using Evolutio.Domain.Repositories.User;
using Evolutio.Exception;
using Evolutio.Exception.ExceptionsBase;

namespace Evolutio.Application.UseCases.User.GetById;
public class GetUserByIdUseCase : IGetUserByIdUseCase
{
    private readonly IUserReadOnlyRepository _readOnlyRepository;
    private readonly IMapper _mapper;
    public GetUserByIdUseCase(IUserReadOnlyRepository readOnlyRepository,
        IMapper mapper)
    {
        _readOnlyRepository = readOnlyRepository;
        _mapper = mapper;
    }
    public async Task<ResponseUserProfileJson> Execute(long id)
    {
        var user = await _readOnlyRepository.GetById(id);

        if (user is null) 
        {
            throw new NotFoundException(ResourceMessagesException.USER_NOT_FOUND);
        }

        var mappedUser = _mapper.Map<ResponseUserProfileJson>(user);

        return mappedUser;
    }
}

