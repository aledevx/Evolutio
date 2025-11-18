using Evolutio.Communication.Responses;

namespace Evolutio.Application.UseCases.Health.DatabaseStatus;
public interface IDatabaseStatusUseCase
{
    Task<ResponseDatabaseStatusJson> Execute();
}
