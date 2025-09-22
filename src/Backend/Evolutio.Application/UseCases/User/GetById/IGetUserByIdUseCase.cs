using Evolutio.Communication.Responses;

namespace Evolutio.Application.UseCases.User.GetById;
public interface IGetUserByIdUseCase
{
    Task<ResponseUserProfileJson> Execute(long id);
}

