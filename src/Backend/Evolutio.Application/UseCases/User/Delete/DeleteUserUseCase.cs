
using Evolutio.Domain.Repositories;
using Evolutio.Domain.Repositories.User;
using Evolutio.Exception;
using Evolutio.Exception.ExceptionsBase;

namespace Evolutio.Application.UseCases.User.Delete;
public class DeleteUserUseCase : IDeleteUserUseCase
{
    private readonly IUserDeleteOnlyRepository _deleteRepository;
    private readonly IUserReadOnlyRepository _readRepository;
    private readonly IUnitOfWork _unitOfWork;
    public DeleteUserUseCase(IUserDeleteOnlyRepository deleteRepository,
        IUserReadOnlyRepository readRepository,
        IUnitOfWork unitOfWork)
    {
        _deleteRepository = deleteRepository;
        _readRepository = readRepository;
        _unitOfWork = unitOfWork;
    }
    public async Task Execute(long id)
    {
        var userExists = await _readRepository.ExistsById(id);
        if (userExists is false)
        {
            throw new NotFoundException(ResourceMessagesException.USER_NOT_FOUND);
        }

        await _deleteRepository.Delete(id);

        await _unitOfWork.CommitAsync();
    }
}

