using Blazored.LocalStorage;
using Evolutio.Communication;
using Evolutio.Web;
using Evolutio.Web.Handlers.Auth.DoLogin;
using Evolutio.Web.Handlers.Auth.Logout;
using Evolutio.Web.Handlers.Health.DatabaseStatus;
using Evolutio.Web.Handlers.Login.DoLogin;
using Evolutio.Web.Handlers.Token.RefreshToken;
using Evolutio.Web.Handlers.User.Profile;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient 
{ 
    BaseAddress = new Uri(Configuration.BackendUrl),
    DefaultRequestHeaders =
    {
        { "Accept", "application/json" }
    }
});

builder.Services.AddBlazoredLocalStorage();
builder.Services.AddMudServices();

builder.Services.AddTransient<IDatabaseStatusHandler, DatabaseStatusHandler>();
builder.Services.AddScoped<IDoLoginHandler, DoLoginHandler>();
builder.Services.AddScoped<ILogoutHandler, LogoutHandler>();
builder.Services.AddScoped<IRefreshTokenHandler, RefreshTokenHandler>();
builder.Services.AddScoped<IUserProfileHandler, UserProfileHandler>();

await builder.Build().RunAsync();
