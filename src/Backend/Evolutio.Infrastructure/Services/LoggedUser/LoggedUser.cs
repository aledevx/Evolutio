using Evolutio.Domain.Entities;
using Evolutio.Domain.Security.Tokens;
using Evolutio.Domain.Services.LoggedUser;
using Evolutio.Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Evolutio.Infrastructure.Services.LoggedUser;
public class LoggedUser : ILoggedUser
{
    private readonly EvolutioDbContext _context;
    private readonly ITokenProvider _tokenProvider;

    public LoggedUser(EvolutioDbContext context, ITokenProvider tokenProvider)
    {
        _context = context;
        _tokenProvider = tokenProvider;
    }
    public async Task<User> User()
    {
        var token = _tokenProvider.Value();
        var tokenHandler = new JwtSecurityTokenHandler();

        var jwtSecurityToken = tokenHandler.ReadJwtToken(token);

        var identifier = jwtSecurityToken.Claims.First(claim => claim.Type == ClaimTypes.Sid).Value;

        var userIdentifier = Guid.Parse(identifier);

        return await _context.Users
            .AsNoTracking()
            .FirstAsync(u => u.UserIdentifier.Equals(userIdentifier)
                && u.Active);
    }
}
