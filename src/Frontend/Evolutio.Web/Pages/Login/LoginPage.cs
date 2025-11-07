using Evolutio.Communication.Requests;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Evolutio.Web.Pages.Login;
public class LoginPage : ComponentBase
{
    #region Properties
    public bool IsBusy { get; set; } = false;
    public RequestLoginJson InputModel { get; set; } = new();

    #endregion

    #region Services

    [Inject]
    public NavigationManager NavigationManager { get; set; } = null!;
    [Inject]
    public ISnackbar Snackbar { get; set; } = null!;

    #endregion

    #region Methods

    public void OnValidSubmitAsync()
    {
        IsBusy = true;

        Snackbar.Add("Submit ok");
    }

    #endregion
}
