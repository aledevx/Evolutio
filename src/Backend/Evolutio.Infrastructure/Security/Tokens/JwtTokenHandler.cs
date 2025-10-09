using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Evolutio.Infrastructure.Security.Tokens;
public class JwtTokenHandler
{
    protected static SymmetricSecurityKey SecurityKey(string signingKey)
    {
        var bytes = Encoding.UTF8.GetBytes(signingKey);

        return new SymmetricSecurityKey(bytes);
    }
}

