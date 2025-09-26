namespace Evolutio.Domain.Repositories.User;
public interface IUserDeleteOnlyRepository
{
    Task Delete(long id);
}
