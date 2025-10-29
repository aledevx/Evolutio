using Evolutio.Communication.Requests;
using Evolutio.Communication.Responses;

namespace Evolutio.Application.UseCases.User.Register;
public interface IRegisterUserUseCase
{
    public Task Execute(RequestRegisterUserJson request);
}

