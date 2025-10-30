using CommonTestUtilities.Requests;
using CommonTestUtilities.Tokens;
using Evolutio.Domain.Enums;
using FluentAssertions;
using System.Net;

namespace WebApi.Test.User.GetById;
public class GetUserByIdInvalidTokenTest : EvolutioClassFixture
{
    private readonly string METHOD = "user";
    private readonly Perfil _userProfile;
    private readonly long _userId;
    public GetUserByIdInvalidTokenTest(CustomWebApplicationFactory factory) : base(factory)
    {
        _userProfile = factory.GetUserProfile();
        _userId = factory.GetUserId();
    }
    [Fact]
    public async Task Error_Token_Invalid()
    {
        var request = RequestUpdateUserJsonBuilder.Build();
        var response = await DoGet(method: $"{METHOD}/{_userId}", token: "invalidToken");

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
    [Fact]
    public async Task Error_Without_Token()
    {
        var request = RequestUpdateUserJsonBuilder.Build();
        var response = await DoGet(method: $"{METHOD}/{_userId}", token: string.Empty);

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
    [Fact]
    public async Task Error_Token_With_User_NotFound()
    {
        var request = RequestUpdateUserJsonBuilder.Build();

        var token = JwtTokenGeneratorBuilder.Build().Generate(Guid.NewGuid(), _userProfile);

        var response = await DoGet(method: $"{METHOD}/{_userId}", token: token);

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}
