using Blazored.LocalStorage;
using Evolutio.Communication.Responses;
using Evolutio.Communication.Routes.EvolutioApi.Auth;
using Evolutio.Web.Extensions;
using Evolutio.Web.Handlers.Auth.Logout;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Evolutio.Web.Layout;
public class MainLayoutPage : LayoutComponentBaseExtension
{
    #region Properties
    public bool IsBusy { get; set; } = false;
    public bool DrawerOpen { get; set; } = true;
    public MudTheme Matrix { get; set; } = default!;
    #endregion

    #region Services

    [Inject]
    public NavigationManager NavigationManager { get; set; } = null!;
    [Inject]
    public ILogoutHandler LogoutHandler { get; set; } = null!;
    [Inject]
    public ISnackbar Snackbar { get; set; } = null!;
    [Inject]
    public ILocalStorageService LocalStorageService { get; set; } = null!;

    #endregion
    #region Methods

    public void ToggleDrawer()
    {
        DrawerOpen = !DrawerOpen;
    }

    public async Task LogoutAsync()
    {
        IsBusy = true;

        try
        {
            var result = await LogoutHandler.Execute();
            if (result is true)
            {
               
                await LocalStorageService.ClearAsync();

                Snackbar.Add($"Deslogado com successo!", Severity.Success);
                NavigationManager.NavigateTo(AuthRoutes.Login);
            }
            else
            {
                var response = result as ResponseErrorJson;
                foreach (var error in response!.Errors)
                {
                    Snackbar.Add(error, Severity.Error);
                }
            }
        }
        catch (Exception ex)
        {
            Snackbar.Add(ex.Message, Severity.Error);
        }
        finally
        {
            IsBusy = false;
        }
    }

    protected override void OnInitialized()
    {
        CreateTheme();
    }
    private void CreateTheme()
    {
        Matrix = new MudTheme()
        {
            PaletteLight = new PaletteLight()
            {
                Primary = Colors.LightGreen.Accent4,
                Secondary = Colors.DeepPurple.Darken1,
                Tertiary = Colors.Teal.Default,
                AppbarBackground = Colors.BlueGray.Darken4,
                Background = Colors.BlueGray.Darken4,
                TextPrimary = Colors.LightGreen.Accent4,
                AppbarText = Colors.LightGreen.Accent4,
            },
            PaletteDark = new PaletteDark()
            {
                Primary = Colors.Blue.Lighten1,
            },
            LayoutProperties = new LayoutProperties()
            {
                DrawerWidthLeft = "260px",
                DrawerWidthRight = "300px",
            },
            Typography = new Typography()
            {
                Default = new DefaultTypography()
                {
                    FontFamily = new[] { "IBM Plex Mono", "monospace" },
                    LetterSpacing = "normal",
                    FontSize = "16px",
                    LineHeight = "24px",
                }
            }
        };
    }

    #endregion
}
