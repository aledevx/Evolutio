using Evolutio.Communication.Requests;
using Evolutio.Communication.Responses;

namespace Evolutio.Application.UseCases.User.Update;
public interface IUpdateUserUseCase
{
    Task<ResponseUserIdJson> Execute(long userId, RequestUpdateUserJson request);
}

