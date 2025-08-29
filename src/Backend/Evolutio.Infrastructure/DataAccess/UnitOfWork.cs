using Evolutio.Domain.Repositories;

namespace Evolutio.Infrastructure.DataAccess;
public class UnitOfWork : IUnitOfWork
{
    private readonly EvolutioDbContext _context;
    public UnitOfWork(EvolutioDbContext context)
    {
        _context = context;
    }
    public async Task CommitAsync() => await _context.SaveChangesAsync();
}

