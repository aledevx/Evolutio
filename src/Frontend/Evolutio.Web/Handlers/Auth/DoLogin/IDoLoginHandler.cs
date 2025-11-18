using Evolutio.Communication.Requests;

namespace Evolutio.Web.Handlers.Auth.DoLogin;
public interface IDoLoginHandler
{
    Task<object> Execute(RequestLoginJson request);
}
