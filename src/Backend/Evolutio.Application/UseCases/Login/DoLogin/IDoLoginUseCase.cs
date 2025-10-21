using Evolutio.Communication.Requests;
using Evolutio.Communication.Responses;

namespace Evolutio.Application.UseCases.Login.DoLogin;
public interface IDoLoginUseCase
{
    Task<ResponseRegisteredUserJson> Execute(RequestLoginJson request);
}
