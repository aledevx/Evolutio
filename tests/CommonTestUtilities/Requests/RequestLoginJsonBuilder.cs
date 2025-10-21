using Bogus;
using Evolutio.Communication.Requests;

namespace CommonTestUtilities.Requests;
public class RequestLoginJsonBuilder
{
    public static RequestLoginJson Build()
    {
        var requestGenerated = new Faker<RequestLoginJson>()
            .RuleFor(request => request.Email, (f) => f.Internet.Email())
            .RuleFor(request => request.Password, (f) => f.Internet.Password());

        return requestGenerated;
    }
}
