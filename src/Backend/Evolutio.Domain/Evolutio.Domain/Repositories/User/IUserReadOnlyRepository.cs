namespace Evolutio.Domain.Repositories.User;
public interface IUserReadOnlyRepository
{
    Task<bool> ExistsByEmail(string email);
    Task<Entities.User?> GetById(long id);
}

