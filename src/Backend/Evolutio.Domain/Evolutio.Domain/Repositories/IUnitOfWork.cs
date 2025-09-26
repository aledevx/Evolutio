namespace Evolutio.Domain.Repositories;
public interface IUnitOfWork
{
    Task CommitAsync();
}
