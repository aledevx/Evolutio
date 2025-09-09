using Evolutio.Domain.Repositories.User;
using Moq;

namespace CommonTestUtilities.Repositories;
public class UserReadOnlyRepositoryBuilder
{
    private readonly Mock<IUserReadOnlyRepository> _repository;
    public UserReadOnlyRepositoryBuilder()
    {
        _repository = new Mock<IUserReadOnlyRepository>();
    }

    public void ExistsByEmail(string email)
    {
        _repository.Setup(repository => repository.ExistsByEmail(email)).ReturnsAsync(true);
    }

    public IUserReadOnlyRepository Build()
    {
        return _repository.Object;
    }
}

