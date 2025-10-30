using Bogus;
using Evolutio.Communication.Enums;
using Evolutio.Communication.Requests;

namespace CommonTestUtilities.Requests;
public class RequestUpdateUserJsonBuilder
{
    public static RequestUpdateUserJson Build() 
    {
        var requestGenerated = new Faker<RequestUpdateUserJson>()
            .RuleFor(request => request.Name, (f) => f.Person.FirstName)
            .RuleFor(request => request.Email, (f, request) => f.Internet.Email(request.Name))
            .RuleFor(request => request.Perfil, () => Perfil.Funcionario);

        return requestGenerated;
    }
}

