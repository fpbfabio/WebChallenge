using RentApp.ApiService.Clients;
using RentApp.ApiService.Server;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire components.
builder.AddServiceDefaults();

builder.Services.AddHttpClient<DriverApiClient>(client =>
{
    client.BaseAddress = new("https+http://driverapi");
});

builder.Services.AddHttpClient<RentalApiClient>(client =>
{
    client.BaseAddress = new("https+http://rentalapi");
});

builder.Services.AddHttpClient<PlanApiClient>(client =>
{
    client.BaseAddress = new("https+http://planapi");
});

builder.Services.AddHttpClient<MotorcycleApiClient>(client =>
{
    client.BaseAddress = new("https+http://motorcycleapi");
});

// Add services to the container.
builder.Services.AddProblemDetails();
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseExceptionHandler();

app.RegisterDriverEndpoints();
app.RegisterPlanEndpoints();
app.RegisterRentalEndpoints();
app.RegisterMotorcycleEndpoints();

app.MapDefaultEndpoints();

app.Run();
