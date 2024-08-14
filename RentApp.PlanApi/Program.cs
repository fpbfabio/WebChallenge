using RentApp.BackDataModelLib;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire components.
builder.AddServiceDefaults();
// Add services to the container.
builder.Services.AddProblemDetails();

var app = builder.Build();


int id = 0;

List<PlanApiDataModel> plans =
[
    new()
    {
        Id = ++id,
        Description = "7 dias com um custo de R$30,00 por dia",
        PricePerDay = 30
    },
    new()
    {
        Id = ++id,
        Description = "15 dias com um custo de R$28,00 por dia",
        PricePerDay = 15
    },
    new()
    {
        Id = ++id,
        Description = "30 dias com um custo de R$22,00 por dia",
        PricePerDay = 30
    },
    new()
    {
        Id = ++id,
        Description = "45 dias com um custo de R$22,00 por dia",
        PricePerDay = 45
    },
];

app.MapGet("/planapi/{id}", async (int id) =>
{
    return await Task.Run(() =>
    {
        return TypedResults.Ok(plans.FirstOrDefault(x => x.Id == id));
    });
});

app.MapGet("/planapi", async () =>
{
    return await Task.Run(() =>
    {
        return TypedResults.Ok(plans);
    });
});

// Configure the HTTP request pipeline.
app.UseExceptionHandler();

app.MapDefaultEndpoints();

app.Run();
