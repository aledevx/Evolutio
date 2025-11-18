using Blazored.LocalStorage;
using Evolutio.Communication.Requests;
using Evolutio.Communication.Responses;
using Evolutio.Web.Extensions;
using Evolutio.Web.Handlers.Auth.DoLogin;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Evolutio.Web.Pages.Login;
public class LoginPage : LayoutComponentBaseExtension
{
    #region Properties
    public bool IsBusy { get; set; } = false;
    public RequestLoginJson InputModel { get; set; } = new();

    #endregion

    #region Services

    [Inject]
    public NavigationManager NavigationManager { get; set; } = null!;
    [Inject]
    public IDoLoginHandler Handler { get; set; } = null!;
    [Inject]
    public ISnackbar Snackbar { get; set; } = null!;
    [Inject]
    public ILocalStorageService LocalStorageService { get; set; } = null!;

    #endregion

    #region Methods

    public async Task OnValidSubmitAsync()
    {
        IsBusy = true;

        try 
        {
            var result = await Handler.Execute(InputModel);
            if (result is ResponseUserNameJson)
            {
                var response = result as ResponseUserNameJson;

                await LocalStorageService.SetItemAsync("userName", response!.Name);
                Snackbar.Add($"Seja bem Vindo {response!.Name}!", Severity.Success);
                NavigationManager.NavigateTo("/home");
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

    protected override async Task OnInitializedAsync()
    {
        var isAuthenticaded = await IsAuthenticaded();
        if (isAuthenticaded)
        {
            NavigationManager.NavigateTo("/home");
        }
    }

    #endregion
}
