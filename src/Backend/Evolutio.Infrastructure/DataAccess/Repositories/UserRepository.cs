using Evolutio.Domain.Entities;
using Evolutio.Domain.Repositories.User;
using Microsoft.EntityFrameworkCore;

namespace Evolutio.Infrastructure.DataAccess.Repositories;
public class UserRepository : IUserWriteOnlyRepository, IUserReadOnlyRepository, IUserUpdateOnlyRepository
{
    private readonly EvolutioDbContext _dbContext;
    public UserRepository(EvolutioDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task Add(User user)
    {
        await _dbContext.Users.AddAsync(user);
    }

    public async Task<bool> ExistsByEmail(string email)
    {
        return await _dbContext.Users.AnyAsync(u => u.Email.Equals(email) && u.Active);
    }

    public async Task<bool> ExistsById(long id)
    {
        return await _dbContext.Users.AnyAsync(u => u.UserIdentifier.Equals(id));
    }

    async Task<User?> IUserUpdateOnlyRepository.GetById(long id)
    {
        return await _dbContext.Users.FirstOrDefaultAsync(u => u.Id.Equals(id) && u.Active);
    }

    async Task<User?> IUserReadOnlyRepository.GetById(long id)
    {
        return await _dbContext.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id.Equals(id) && u.Active);
    }

    public void Update(User user)
    {
        _dbContext.Users.Update(user);
    }
}

