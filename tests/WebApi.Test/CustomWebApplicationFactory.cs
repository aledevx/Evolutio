using CommonTestUtilities.Entities;
using Evolutio.Domain.Enums;
using Evolutio.Infrastructure.DataAccess;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace WebApi.Test;
public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    private Evolutio.Domain.Entities.User _user = default!;
    private Evolutio.Domain.Entities.RefreshToken _refreshToken = default!;
    private string _password = string.Empty;
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Test").ConfigureServices(services =>
        {
            // Localiza o DbContext registrado originalmente
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<EvolutioDbContext>)
            );

            // Remove caso exista (o que conecta ao banco real)
            if (descriptor is not null)
                services.Remove(descriptor);

            var provider = services.AddEntityFrameworkInMemoryDatabase().BuildServiceProvider(); // Agora reconhecido

            // Registra o novo DbContext em memória
            services.AddDbContext<EvolutioDbContext>(options =>
            {
                options.UseInMemoryDatabase("InMemoryDbForTesting");
                options.UseInternalServiceProvider(provider);
            });

            using var scope = services.BuildServiceProvider().CreateScope();

            var dbContext = scope.ServiceProvider.GetRequiredService<EvolutioDbContext>();

            CreateDatabase(dbContext);
        });
    }
    public string GetName() => _user.Name;
    public string GetEmail() => _user.Email;
    public string GetPassword() => _password;
    public Perfil GetUserProfile() => _user.Perfil;
    public Guid GetUserIdentifier() => _user.UserIdentifier;
    public long GetUserId() => _user.Id;
    private void CreateDatabase(EvolutioDbContext dbContext)
    {
        (_user, _password) = UserBuilder.Build();

        _refreshToken = RefreshTokenBuilder.Build(_user);

        // Garante que o banco esteja "limpo" antes de recriar
        dbContext.Database.EnsureDeleted();

        // Adiciona o usuário criado ao banco
        dbContext.Users.Add(_user);

        // Adiciona o refresh token ao banco
        dbContext.RefreshTokens.Add(_refreshToken);

        // Persiste as alterações
        dbContext.SaveChanges();
    }
}