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
    public void ExistsById(long id) 
    {
        _repository.Setup(repository => repository.ExistsById(id)).ReturnsAsync(true);
    }

    public void GetById(User user) 
    {
        _repository.Setup(repository => repository.GetById(user.Id)).ReturnsAsync(user);
    }
    public void GetByEmail(User user)
    {
        _repository.Setup(repository => repository.GetByEmail(user.Email)).ReturnsAsync(user);
    }

    public IUserReadOnlyRepository Build()
    {
        return _repository.Object;
    }
}

