using Evolutio.Communication.Responses;

namespace Evolutio.Application.UseCases.User.Profile;
public interface IGetUserProfileUseCase
{
    Task<ResponseUserProfileJson> Execute();
}