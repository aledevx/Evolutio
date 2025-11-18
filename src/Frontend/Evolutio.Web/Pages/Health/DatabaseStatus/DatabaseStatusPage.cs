using Evolutio.Communication.Responses;
using Evolutio.Communication.Routes.EvolutioApi.Auth;
using Evolutio.Web.Extensions;
using Evolutio.Web.Handlers.Health.DatabaseStatus;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Evolutio.Web.Pages.Health.DatabaseStatus;
public class DatabaseStatusPage : LayoutComponentBaseExtension
{
    #region Properties
    public bool IsBusy { get; set; } = false;
    public ResponseDatabaseStatusJson Model { get; set; } = new();
    #endregion

    #region Services
    [Inject]
    public IDatabaseStatusHandler Handler { get; set; } = null!;
    [Inject]
    public ISnackbar Snackbar { get; set; } = null!;
    [Inject]
    public NavigationManager NavigationManager { get; set; } = null!;
    #endregion

    #region Methods
    protected override async Task OnInitializedAsync() 
    {
        IsBusy = true;
        try
        {
            var result = await Handler.Execute();
            if (result is ResponseDatabaseStatusJson)
            {
                var data = result as ResponseDatabaseStatusJson;
                Model = data!;
            }
            else 
            {
                var errors = result as ResponseErrorJson;
                foreach (var error in errors!.Errors)
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
    #endregion
}
