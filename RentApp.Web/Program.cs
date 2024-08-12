using Radzen;
using RentApp.Web;
using RentApp.Web.Components.Adapters;
using RentApp.Web.Components.Data.DriverProfile.DataSource;
using RentApp.Web.Components;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.IdentityModel.Tokens.Jwt;
using RentApp.Web.Components.Features.Interfaces;
using RentApp.Web.Components.Features.RegisterProfile.ViewModel;
using RentApp.Web.Components.Features.Rent.ViewModel;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire components.
builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddOutputCache();

builder.Services.AddHttpClient<WeatherApiClient>(client =>
    {
        // This URL uses "https+http://" to indicate HTTPS is preferred over HTTP.
        // Learn more about service discovery scheme resolution at https://aka.ms/dotnet/sdschemes.
        client.BaseAddress = new("https+http://apiservice");
    });

builder.Services.AddHttpClient<DriverProfileRemoteDataSource>(client =>
    {
        // This URL uses "https+http://" to indicate HTTPS is preferred over HTTP.
        // Learn more about service discovery scheme resolution at https://aka.ms/dotnet/sdschemes.
        client.BaseAddress = new("https+http://apiservice");
    });

builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
                .AddKeycloakOpenIdConnect(
                    "keycloak", 
                    realm: "rentapp", 
                    options =>
                    {
                        options.ClientId = "webfrontend";
                        options.ResponseType = OpenIdConnectResponseType.Code;
                        options.RequireHttpsMetadata = false;
                        options.TokenValidationParameters.NameClaimType = JwtRegisteredClaimNames.Name;
                        options.SaveTokens = true;
                        options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    })
                .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme);
builder.Services.AddCascadingAuthenticationState();

builder.Services.AddRadzenComponents();

builder.Services.AddScoped<IRentViewModel, RentViewModel>();
builder.Services.AddScoped<IRegisterProfileViewModel, RegisterProfileViewModel>();
builder.Services.AddTransient<IDriverProfileGateway, DriverProfileRepository>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.UseOutputCache();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.MapDefaultEndpoints();

app.Run();
