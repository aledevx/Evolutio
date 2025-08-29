using Evolutio.Domain.Entities;
using Evolutio.Domain.Repositories.User;

namespace Evolutio.Infrastructure.DataAccess.Repositories;
public class UserRepository : IUserWriteOnlyRepository
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
}

