using Azure;
using Evolutio.Exception;
using FluentAssertions;
using System.Globalization;
using System.Net;
using System.Text.Json;
using WebApi.Test.InlineData;

namespace WebApi.Test.User.Delete;
public class DeleteUserTest : EvolutioClassFixture
{
    private readonly string METHOD = "user";
    private readonly long _userId;
    public DeleteUserTest(CustomWebApplicationFactory factory) : base(factory)
    {
        _userId = factory.GetUserId();
    }
    [Fact]
    public async Task Success() 
    {
        var result = await DoDelete(method: $"{METHOD}/{_userId}");

        result.StatusCode.Should().Be(HttpStatusCode.NoContent);

    }
    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_User_Not_Found(string culture) 
    {
        var result = await DoDelete(method: $"{METHOD}/{2}", culture: culture);

        result.StatusCode.Should().Be(HttpStatusCode.NotFound);

        await using var responseBody = await result.Content.ReadAsStreamAsync();
        var responseData = await JsonDocument.ParseAsync(responseBody);

        var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();

        var expectedMessage = ResourceMessagesException.ResourceManager.GetString("USER_NOT_FOUND", new CultureInfo(culture));

        errors.Should().ContainSingle().And.Contain(error => error.GetString()!.Equals(expectedMessage));
    }
}

