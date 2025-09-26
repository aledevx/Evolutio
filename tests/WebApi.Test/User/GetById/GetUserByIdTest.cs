using Evolutio.Exception;
using FluentAssertions;
using System.Globalization;
using System.Net;
using System.Text.Json;
using WebApi.Test.InlineData;

namespace WebApi.Test.User.GetById;
public class GetUserByIdTest : EvolutioClassFixture
{
    private readonly string METHOD = "user";
    private readonly long _userId;
    private readonly string _userName;
    private readonly string _userEmail;
    public GetUserByIdTest(CustomWebApplicationFactory factory) : base(factory)
    {
        _userId = factory.GetUserId();
        _userName = factory.GetName();
        _userEmail = factory.GetEmail();
    }
    [Fact]
    public async Task Success()
    {
        var response = await DoGet(method: $"{METHOD}/{_userId}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        await using var responseBody = await response.Content.ReadAsStreamAsync();
        var responseData = await JsonDocument.ParseAsync(responseBody);

        responseData.RootElement.GetProperty("name").GetString()
    .Should().NotBeNullOrWhiteSpace().And.Be(_userName);

        responseData.RootElement.GetProperty("email").GetString()
    .Should().NotBeNullOrWhiteSpace().And.Be(_userEmail);

        responseData.RootElement.GetProperty("active").GetBoolean()
    .Should().Be(true);
    }
    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_User_Not_Found(string culture)
    {
        var response = await DoGet(method: $"{METHOD}/{2}", culture: culture);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        await using var responseBody = await response.Content.ReadAsStreamAsync();
        var responseData = await JsonDocument.ParseAsync(responseBody);

        var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();

        var expectedMessage = ResourceMessagesException.ResourceManager.GetString("USER_NOT_FOUND", new CultureInfo(culture));

        errors.Should().ContainSingle().And.Contain(error => error.GetString()!.Equals(expectedMessage));
    }
}

