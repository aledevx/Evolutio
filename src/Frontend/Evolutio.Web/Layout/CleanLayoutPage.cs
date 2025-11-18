using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Evolutio.Web.Layout;
public class CleanLayoutPage : LayoutComponentBase
{
    #region Properties
    public bool DrawerOpen { get; set; } = true;
    public MudTheme VoidTheme { get; set; } = default!;
    #endregion

    #region Methods
    public void ToggleDrawer()
    {
        DrawerOpen = !DrawerOpen;
    }
    protected override void OnInitialized()
    {
        VoidTheme = new MudTheme()
        {
            PaletteLight = new PaletteLight()
            {
                Primary = Colors.Green.Darken3,
                Secondary = Colors.DeepPurple.Darken1,
                Tertiary = Colors.Teal.Default,
                AppbarBackground = Colors.Blue.Darken4,
                Background = Colors.Gray.Darken4
            },
            PaletteDark = new PaletteDark()
            {
                Primary = Colors.Blue.Lighten1
            },
            LayoutProperties = new LayoutProperties()
            {
                DrawerWidthLeft = "260px",
                DrawerWidthRight = "300px"
            }
        };
    }
    #endregion
}
