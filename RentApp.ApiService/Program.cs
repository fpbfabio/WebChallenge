using Microsoft.EntityFrameworkCore;
using RentApp.ApiService.Contexts;
using RentApp.ApiService.Models;

const string CONNECTION_STRING = "mongodb://127.0.0.1:27017/?directConnection=true&serverSelectionTimeoutMS=2000&appName=mongosh+2.2.15";

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire components.
builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddProblemDetails();
builder.Services.AddDbContext<DriverProfileDb>(opt =>
    opt.UseMongoDB(CONNECTION_STRING, "rent_app"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseExceptionHandler();

app.MapGet("/driverprofiles", async (DriverProfileDb db) =>
    await db.Items.ToListAsync());

app.MapGet("/driverprofiles/{id}", async (string id, DriverProfileDb db) =>
    await db.Items.FindAsync(id)
        is DriverProfile profile
            ? Results.Ok(profile)
            : Results.NotFound());

app.MapPost("/driverprofiles", async (DriverProfile profile, DriverProfileDb db) =>
{
    db.Items.Add(profile);
    await db.SaveChangesAsync();

    return Results.Created($"/driverprofiles/{profile.Id}", profile);
});

app.MapPut("/driverprofiles/{id}", async (string id, DriverProfile inputProfile, DriverProfileDb db) =>
{
    var profile = await db.Items.FindAsync(id);

    if (profile is null) return Results.NotFound();

    profile.Name = inputProfile.Name;

    await db.SaveChangesAsync();

    return Results.NoContent();
});

app.MapDelete("/driverprofiles/{id}", async (string id, DriverProfileDb db) =>
{
    if (await db.Items.FindAsync(id) is DriverProfile todo)
    {
        db.Items.Remove(todo);
        await db.SaveChangesAsync();
        return Results.NoContent();
    }

    return Results.NotFound();
});

app.MapDefaultEndpoints();

app.Run();
