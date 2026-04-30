using Evolutio.Communication.Requests;
using Evolutio.Communication.Responses;
using Evolutio.Web.Extensions;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Net.Http.Json;

namespace Evolutio.Web.Pages.User.Register;

public class RegisterPage : LayoutComponentBaseExtension
{
    #region Properties
    public RequestRegisterUserJson InputModel { get; set; } = new();
    public bool IsBusy { get; set; } = false;
    public bool RegisterFailed { get; set; } = false;
    #endregion

    #region Services
    [Inject]
    public HttpClient HttpClient { get; set; } = null!;
    [Inject]
    public NavigationManager NavigationManager { get; set; } = null!;
    [Inject]
    public ISnackbar Snackbar { get; set; } = null!;
    #endregion

    #region Methods

    protected override async Task OnInitializedAsync()
    {
        var isAuthenticaded = await IsAuthenticaded();
        if (isAuthenticaded)
        {
            NavigationManager.NavigateTo("/home");
        }
    }

    public async Task OnValidSubmitAsync()
    {
        IsBusy = true;
        RegisterFailed = false;
        StateHasChanged();

        try
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "/user");
            request.Content = JsonContent.Create(InputModel);

            var result = await HttpClient.SendAsync(request);

            if (result.IsSuccessStatusCode)
            {
                Snackbar.Add("Conta criada com sucesso!", Severity.Success);
                NavigationManager.NavigateTo("/login");
            }
            else
            {
                RegisterFailed = true;
                var errorResponse = await result.Content.ReadFromJsonAsync<ResponseErrorJson>();
                if (errorResponse != null && errorResponse.Errors != null)
                {
                    foreach (var error in errorResponse.Errors)
                    {
                        Snackbar.Add(error, Severity.Error);
                    }
                }
                else
                {
                    Snackbar.Add("Erro desconhecido ao cadastrar.", Severity.Error);
                }
            }
        }
        catch (Exception ex)
        {
            RegisterFailed = true;
            Snackbar.Add(ex.Message, Severity.Error);
        }
        finally
        {
            IsBusy = false;
            StateHasChanged();
        }
    }
    #endregion
}
