using Evolutio.Domain.Entities;
using Evolutio.Domain.Repositories.User;
using Microsoft.EntityFrameworkCore;

namespace Evolutio.Infrastructure.DataAccess.Repositories;
public class UserRepository : IUserWriteOnlyRepository, IUserReadOnlyRepository
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
        return await _dbContext.Users.AnyAsync(u => u.Email == email);
    }
}

