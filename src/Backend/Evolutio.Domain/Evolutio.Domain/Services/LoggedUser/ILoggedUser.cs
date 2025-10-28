using Evolutio.Domain.Entities;

namespace Evolutio.Domain.Services.LoggedUser;
public interface ILoggedUser
{
    public Task<User> User();
}
