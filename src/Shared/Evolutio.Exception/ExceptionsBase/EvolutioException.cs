using System.Net;

namespace Evolutio.Exception.ExceptionsBase;
public abstract class EvolutioException : SystemException
{
    protected EvolutioException(string message) : base(message)
    {
        
    }
    public abstract IList<string> GetErrorMessages();
    public abstract HttpStatusCode GetStatusCode();
}

