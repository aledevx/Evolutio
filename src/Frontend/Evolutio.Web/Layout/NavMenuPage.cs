using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;

namespace Evolutio.Web.Layout;
public class NavMenuPage : ComponentBase
{
    #region Properties
    public bool CollapseNavMenu { get; set; } = true;
    public string UserName { get; set; } = string.Empty;
    public string? NavMenuCssClass => CollapseNavMenu ? "collapse" : null;
    #endregion

    #region Services
    [Inject]
    public ILocalStorageService LocalStorage { get; set; } = null!;
    #endregion

    #region Methods

    public void ToggleNavMenu()
    {
        CollapseNavMenu = !CollapseNavMenu;
    }
    protected override async Task OnInitializedAsync()
    {
        UserName = await LocalStorage.GetItemAsStringAsync("userName") ?? "Logged Out";
    }
    #endregion
}
