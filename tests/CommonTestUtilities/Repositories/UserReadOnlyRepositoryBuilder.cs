using Evolutio.Domain.Entities;
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

    public UserReadOnlyRepositoryBuilder GetById(User? user) 
    {
        if(user is not null)
        _repository.Setup(repository => repository.GetById(user.Id)).ReturnsAsync(user);

        return this;
    }

    public IUserReadOnlyRepository Build()
    {
        return _repository.Object;
    }
}

