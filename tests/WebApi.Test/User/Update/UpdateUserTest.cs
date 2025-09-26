using CommonTestUtilities.Requests;
using Evolutio.Exception;
using FluentAssertions;
using System.Globalization;
using System.Net;
using System.Text.Json;
using WebApi.Test.InlineData;

namespace WebApi.Test.User.Update;
public class UpdateUserTest : EvolutioClassFixture
{
    private readonly string METHOD = "user";
    private readonly long _userId;
    private readonly string _userEmail;
    public UpdateUserTest(CustomWebApplicationFactory factory) : base(factory)
    {
        _userId = factory.GetUserId();
        _userEmail = factory.GetEmail();
    }
    [Fact]
    public async Task Success()
    {
        var request = RequestUpdateUserJsonBuilder.Build();

        var response = await DoPut(method: $"{METHOD}/{_userId}", request: request);

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        await using var responseBodyPutMethod = await response.Content.ReadAsStreamAsync();
        var responseData = await JsonDocument.ParseAsync(responseBodyPutMethod);

        responseData.RootElement.GetProperty("id").GetInt64()
        .Should().Be(_userId);

        response = await DoGet(method: $"{METHOD}/{_userId}");
        
        await using var responseBodyGetMethod = await response.Content.ReadAsStreamAsync();
        responseData = await JsonDocument.ParseAsync(responseBodyGetMethod);

        responseData.RootElement.GetProperty("name").GetString()
        .Should().NotBeNullOrWhiteSpace().And.Be(request.Name);

        responseData.RootElement.GetProperty("email").GetString()
        .Should().NotBeNullOrWhiteSpace().And.Be(request.Email);

        responseData.RootElement.GetProperty("active").GetBoolean()
        .Should().Be(true);

    }
    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Email_Already_Exist(string culture)
    {
        var request = RequestUpdateUserJsonBuilder.Build();
        request.Email = _userEmail;

        var response = await DoPut(method: $"{METHOD}/{_userId}", request: request, culture: culture);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        await using var responseBody = await response.Content.ReadAsStreamAsync();
        var responseData = await JsonDocument.ParseAsync(responseBody);

        var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();

        var expectedMessage = ResourceMessagesException.ResourceManager.GetString("EMAIL_ALREADY_REGISTERED", new CultureInfo(culture));

        errors.Should().ContainSingle().And.Contain(error => error.GetString()!.Equals(expectedMessage));
    }
}
