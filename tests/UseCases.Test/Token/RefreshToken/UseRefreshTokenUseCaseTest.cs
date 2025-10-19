using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Tokens;
using Evolutio.Application.UseCases.Token.RefreshToken;
using Evolutio.Communication.Requests;
using Evolutio.Exception;
using Evolutio.Exception.ExceptionsBase;
using FluentAssertions;
using System;

namespace UseCases.Test.Token.RefreshToken;
public class UseRefreshTokenUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        (var user, _) = UserBuilder.Build();
        var refreshToken = RefreshTokenBuilder.Build(user);

        var useCase = CreateUseCase(refreshToken);

        var result = await useCase.Execute(new RequestNewToken()
        {
            RefreshToken = refreshToken.Value
        });

        result.Should().NotBeNull();
        result.AccessToken.Should().NotBeNullOrEmpty();
        result.RefreshToken.Should().NotBeNullOrEmpty();
    }
    [Fact]
    public async Task Error_RefreshToken_NotFound()
    {
        (var user, _) = UserBuilder.Build();
        var refreshToken = RefreshTokenBuilder.Build(user);

        var useCase = CreateUseCase(refreshToken);

        refreshToken.Value = RefreshTokenGeneratorBuilder.Build().Generate();

        Func<Task> act = async () => await useCase.Execute(new RequestNewToken() { RefreshToken = refreshToken.Value });

        (await act.Should().ThrowAsync<RefreshTokenNotFoundException>())
            .Where(e => e.GetErrorMessages().Count == 1 &&
                        e.GetErrorMessages().Contains(ResourceMessagesException.INVALID_SESSION));

    }
    [Fact]
    public async Task Error_RefreshToken_Expired()
    {
        (var user, _) = UserBuilder.Build();
        var refreshToken = RefreshTokenBuilder.Build(user);
        refreshToken.CreatedOn = DateTime.UtcNow.AddDays(-10);

        var useCase = CreateUseCase(refreshToken);

        Func<Task> act = async () => await useCase.Execute(new RequestNewToken() { RefreshToken = refreshToken.Value });

        (await act.Should().ThrowAsync<RefreshTokenExpiredException>())
            .Where(e => e.GetErrorMessages().Count == 1 &&
                e.GetErrorMessages().Contains(ResourceMessagesException.EXPIRED_SESSION));

    }
    private static UseRefreshTokenUseCase CreateUseCase(Evolutio.Domain.Entities.RefreshToken refreshToken) 
    {
        var tokenRepository = new TokenRepositoryBuilder().Get(refreshToken).Build();
        var unitOfWork = UnitOfWorkBuilder.Build();
        var accessTokenGenerator = JwtTokenGeneratorBuilder.Build();
        var refreshTokenGenerator = RefreshTokenGeneratorBuilder.Build();

        return new UseRefreshTokenUseCase(tokenRepository, unitOfWork, accessTokenGenerator, refreshTokenGenerator);
    }
}
