namespace Evolutio.Domain.Repositories.User;
public interface IUserReadOnlyRepository
{
    Task<bool> ExistsByEmail(string email);
    Task<bool> ExistsById(long id);
    Task<Entities.User?> GetById(long id);
    Task<Entities.User?> GetByEmail(string email);
}

