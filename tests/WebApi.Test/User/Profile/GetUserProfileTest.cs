using CommonTestUtilities.Tokens;
using Evolutio.Domain.Enums;
using FluentAssertions;
using System.Net;
using System.Text.Json;

namespace WebApi.Test.User.Profile;
public class GetUserProfileTest : EvolutioClassFixture
{
    private readonly string METHOD = "user/profile";
    private readonly string _userName;
    private readonly string _userEmail;
    private readonly Guid _userIdentifier;
    private readonly Perfil _perfil;
    public GetUserProfileTest(CustomWebApplicationFactory factory) : base(factory)
    {
        _userName = factory.GetName();
        _userEmail = factory.GetEmail();
        _userIdentifier = factory.GetUserIdentifier();
        _perfil = factory.GetUserProfile();
    }
    [Fact]
    public async Task Success()
    {
        var token = JwtTokenGeneratorBuilder.Build().Generate(_userIdentifier, _perfil);

        var response = await DoGet(method: $"{METHOD}", token:token);

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        await using var responseBody = await response.Content.ReadAsStreamAsync();
        var responseData = await JsonDocument.ParseAsync(responseBody);

        responseData.RootElement.GetProperty("name").GetString()
            .Should()
            .NotBeNullOrWhiteSpace().And.Be(_userName);

        responseData.RootElement.GetProperty("email").GetString()
            .Should()
            .NotBeNullOrWhiteSpace().And.Be(_userEmail);

        responseData.RootElement.GetProperty("active").GetBoolean().Should().Be(true);
    }
}
