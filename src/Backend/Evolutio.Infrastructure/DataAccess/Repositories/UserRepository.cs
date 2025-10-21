using Evolutio.Domain.Entities;
using Evolutio.Domain.Repositories.User;
using Microsoft.EntityFrameworkCore;

namespace Evolutio.Infrastructure.DataAccess.Repositories;
public class UserRepository : IUserWriteOnlyRepository, IUserReadOnlyRepository, IUserUpdateOnlyRepository, IUserDeleteOnlyRepository
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
        return await _dbContext.Users.AnyAsync(u => u.Id.Equals(id));
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

    public async Task Delete(long id)
    {
        var user = await _dbContext.Users.FindAsync(id);

        _dbContext.Users.Remove(user!);
    }

    public async Task<User?> GetByEmail(string email)
    {
        var user = await _dbContext.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Email.Equals(email) && u.Active);
        return user;
    }
}

