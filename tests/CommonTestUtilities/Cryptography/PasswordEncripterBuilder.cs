using Evolutio.Domain.Security.Cryptography;
using Evolutio.Infrastructure.Security.Cryptography;

namespace CommonTestUtilities.Cryptography;
public class PasswordEncripterBuilder
{
    public static IPasswordEncripter Build()
    {
        return new BCryptNet();
    }
}

