using Evolutio.Domain.Repositories.User;
using Moq;

namespace CommonTestUtilities.Repositories;
public class UserDeleteOnlyRepositoryBuilder
{
    public static IUserDeleteOnlyRepository Build() 
    {
        var mock = new Mock<IUserDeleteOnlyRepository>();

        return mock.Object;
    }
}

