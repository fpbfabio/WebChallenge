using Microsoft.EntityFrameworkCore;
using RentApp.BackDataModelLib;
using RentApp.MotorcycleApi.Contexts;
using RentApp.MotorcycleApi.Converter;
using RentApp.MotorcycleApi.Models;

const string PATH = "/motorcycleapi";

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire components.
builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddProblemDetails();
string? connectionString = builder.Configuration.GetConnectionString("mongodb");
if (connectionString != null)
{
    builder.Services.AddDbContext<MotorcycleDb>(opt =>
        opt.UseMongoDB(connectionString, "rent_app"));
}
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseExceptionHandler();

app.MapGet(PATH, async (MotorcycleDb db) =>
    await db.Items.ToListAsync());

app.MapGet(PATH + "/{id}", async (string id, MotorcycleDb db) =>
    await db.Items.FindAsync(id)
        is MotorcycleDatabaseModel motorcycle
            ? Results.Ok(motorcycle)
            : Results.NotFound());

app.MapPost(PATH, async (MotorcycleApiDataModel motorcycle, MotorcycleDb db) =>
{
    db.Items.Add(ModelConverter.ToDatabaseModel(motorcycle));
    await db.SaveChangesAsync();

    return Results.Created($"{PATH}/{motorcycle.LicensePlate}", motorcycle);
});

app.MapPut(PATH, async (MotorcycleApiDataModel inputMotorcycle, MotorcycleDb db) =>
{
    var motorcycle = await db.Items.FindAsync(inputMotorcycle.LicensePlate);

    if (motorcycle is null) return Results.NotFound();

    motorcycle.Identifier = inputMotorcycle.Identifier;
    motorcycle.ModelName = inputMotorcycle.ModelName;
    motorcycle.ActiveUserId = inputMotorcycle.ActiveUserlId;
    motorcycle.Year = inputMotorcycle.Year;

    await db.SaveChangesAsync();

    return TypedResults.Ok();
});

app.MapDelete(PATH + "/{id}", async (string id, MotorcycleDb db) =>
{
    if (await db.Items.FindAsync(id) is MotorcycleDatabaseModel motorcycle)
    {
        if (motorcycle.ActiveUserId is null)
        {
            db.Items.Remove(motorcycle);
            await db.SaveChangesAsync();
            return TypedResults.Ok();
        }
        else
        {
            return TypedResults.BadRequest("A moto está alugada");
        }
    }

    return Results.NotFound();
});

app.MapDefaultEndpoints();

app.Run();
