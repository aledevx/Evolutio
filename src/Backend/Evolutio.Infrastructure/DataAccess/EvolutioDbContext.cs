using Evolutio.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Evolutio.Infrastructure.DataAccess;
public class EvolutioDbContext : DbContext
{
    public EvolutioDbContext(DbContextOptions options) : base(options)
    { }

    public DbSet<User> Users { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Aplica automaticamente todas as classes de configuração (IEntityTypeConfiguration)
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(EvolutioDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }
}
