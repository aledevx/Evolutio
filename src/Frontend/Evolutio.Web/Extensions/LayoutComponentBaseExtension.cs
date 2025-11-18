using Evolutio.Communication.Responses;
using Evolutio.Web.Handlers.Token.RefreshToken;
using Evolutio.Web.Handlers.User.Profile;
using Microsoft.AspNetCore.Components;

namespace Evolutio.Web.Extensions;
public class LayoutComponentBaseExtension : LayoutComponentBase
{
    #region Services
    [Inject]
    public IUserProfileHandler ProfileHandler { get; set; } = null!;
    [Inject]
    public IRefreshTokenHandler RefreshTokenHandler { get; set; } = null!;
    #endregion
    public async Task<bool> IsAuthenticaded() 
    {
        var profileResult = await ProfileHandler.Execute();
        if (profileResult is ResponseErrorJson)
        {
            var tokensResult = await RefreshTokenHandler.Execute();
            if (tokensResult is ResponseErrorJson)
            {
                return false;
            }
        return true;
        }
        return true;
    }
}


