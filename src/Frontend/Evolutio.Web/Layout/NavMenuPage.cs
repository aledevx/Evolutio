using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Evolutio.Web.Layout;
public class NavMenuPage : ComponentBase
{
    #region Properties
    [Parameter]
    public bool SideMenuOpen { get; set; }
    public DrawerClipMode clipMode = DrawerClipMode.Always;
    public Breakpoint breakpoint = Breakpoint.Lg;
    public bool preserveOpenState = true;
    public string UserName { get; set; } = string.Empty;
    #endregion

    #region Services
    [Inject]
    public ILocalStorageService LocalStorage { get; set; } = null!;
    #endregion

    #region Methods
    protected override async Task OnInitializedAsync()
    {
        UserName = await LocalStorage.GetItemAsStringAsync("userName") ?? "Logged Out";
    }
    #endregion
}
