using Evolutio.Domain.Entities;
using Evolutio.Domain.Repositories.User;
using Moq;

namespace CommonTestUtilities.Repositories;
public class UserUpdateOnlyRepositoryBuilder
{
    private readonly Mock<IUserUpdateOnlyRepository> _repository;
    public UserUpdateOnlyRepositoryBuilder()
    {
        _repository = new Mock<IUserUpdateOnlyRepository>();
    }
    public UserUpdateOnlyRepositoryBuilder GetById(User user) 
    {
        _repository.Setup(r => r.GetById(user.Id)).ReturnsAsync(user);

        return this;
    }
    public IUserUpdateOnlyRepository Build()
    {
        return _repository.Object;
    }

}
