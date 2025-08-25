using System.Net;

namespace Evolutio.Exception.ExceptionsBase;
public class ErrorOnValidationException : EvolutioException
{
    private readonly IList<string> _errorMessages;
    public ErrorOnValidationException(IList<string> errorMessages) : base(string.Empty)
    {
        _errorMessages = errorMessages;
    }
    public override IList<string> GetErrorMessages()
    {
        return _errorMessages;
    }

    public override HttpStatusCode GetStatusCode()
    {
        return HttpStatusCode.BadRequest;
    }
}

