using System.Net;

namespace Evolutio.Exception.ExceptionsBase;
public class NotFoundException : EvolutioException
{
    public NotFoundException(string message) : base(message)
    {
        
    }
    public override IList<string> GetErrorMessages()
    {
        return [Message];
    }

    public override HttpStatusCode GetStatusCode()
    {
        throw new NotImplementedException();
    }
}

