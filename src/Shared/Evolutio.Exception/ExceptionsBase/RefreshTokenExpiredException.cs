using System.Net;

namespace Evolutio.Exception.ExceptionsBase;
public class RefreshTokenExpiredException : EvolutioException
{
    public RefreshTokenExpiredException() : base(ResourceMessagesException.EXPIRED_SESSION)
    {
    }
    public override IList<string> GetErrorMessages()
    {
        return [Message];
    }

    public override HttpStatusCode GetStatusCode()
    {
        return HttpStatusCode.Forbidden;
    }
}

