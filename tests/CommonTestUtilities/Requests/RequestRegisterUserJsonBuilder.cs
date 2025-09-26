using Bogus;
using Evolutio.Communication.Requests;

namespace CommonTestUtilities.Requests;
public class RequestRegisterUserJsonBuilder
{
    public static RequestRegisterUserJson Build(int passwordLenght = 10) 
    {
        var requestGenerated = new Faker<RequestRegisterUserJson>()
            .RuleFor(request => request.Name, (f) => f.Person.FirstName)
            .RuleFor(request => request.Password, (f) => f.Internet.Password(passwordLenght))
            .RuleFor(request => request.Email, (f, request) => f.Internet.Email(request.Name));

        return requestGenerated;
    }
}

