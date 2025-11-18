namespace Evolutio.Web.Handlers.Health.DatabaseStatus;
public interface IDatabaseStatusHandler
{
    Task<object> Execute();
}
