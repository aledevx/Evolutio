using System.Net;

namespace Evolutio.Exception.ExceptionsBase;
public class NoPermissionException : EvolutioException
{
    public NoPermissionException(string message) : base(message)
    {
        
    }
    public override IList<string> GetErrorMessages()
    {
        return [Message];
    }

    public override HttpStatusCode GetStatusCode()
    {
        return HttpStatusCode.Unauthorized;
    }
}
