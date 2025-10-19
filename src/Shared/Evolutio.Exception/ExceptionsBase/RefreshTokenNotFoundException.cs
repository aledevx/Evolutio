using System.Net;

namespace Evolutio.Exception.ExceptionsBase;
public class RefreshTokenNotFoundException : EvolutioException
{
    public RefreshTokenNotFoundException() : base(ResourceMessagesException.INVALID_SESSION)
    {

    }
    public override IList<string> GetErrorMessages()
    {
        return [Message];
    }

    public override HttpStatusCode GetStatusCode()
    {
        return HttpStatusCode.NotFound;
    }
}

