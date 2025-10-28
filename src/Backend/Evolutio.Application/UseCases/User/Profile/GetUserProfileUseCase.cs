using AutoMapper;
using Evolutio.Communication.Responses;
using Evolutio.Domain.Services.LoggedUser;

namespace Evolutio.Application.UseCases.User.Profile;
public class GetUserProfileUseCase : IGetUserProfileUseCase
{
    private readonly ILoggedUser _loggedUser;
    private readonly IMapper _mapper;
    public GetUserProfileUseCase(ILoggedUser loggedUser,
        IMapper mapper)
    {
        _loggedUser = loggedUser;
        _mapper = mapper;
    }
    public async Task<ResponseUserProfileJson> Execute()
    {
        var user = await _loggedUser.User();

        var response = _mapper.Map<ResponseUserProfileJson>(user);

        return response;
    }
}
