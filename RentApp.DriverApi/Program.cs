using Microsoft.EntityFrameworkCore;
using RentApp.DriverApi.Contexts;
using RentApp.DriverApi.Models;

const string PATH = "/driverapi";

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire components.
builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddProblemDetails();
string? connectionString = builder.Configuration.GetConnectionString("mongodb");
if (connectionString != null)
{
    builder.Services.AddDbContext<DriverProfileDb>(opt =>
        opt.UseMongoDB(connectionString, "rent_app"));
}
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseExceptionHandler();

app.MapGet(PATH, async (DriverProfileDb db) =>
    await db.Items.ToListAsync());

app.MapGet(PATH + "/{id}", async (string id, DriverProfileDb db) =>
    await db.Items.FindAsync(id)
        is DriverProfile profile
            ? Results.Ok(profile)
            : Results.NotFound());

app.MapPost(PATH, async (DriverProfile profile, DriverProfileDb db) =>
{
    db.Items.Add(profile);
    await db.SaveChangesAsync();

    return Results.Created($"{PATH}/{profile.Id}", profile);
});

app.MapPut(PATH + "/{id}", async (string id, DriverProfile inputProfile, DriverProfileDb db) =>
{
    var profile = await db.Items.FindAsync(id);

    if (profile is null) return Results.NotFound();

    profile.Name = inputProfile.Name;

    await db.SaveChangesAsync();

    return Results.NoContent();
});

app.MapDelete(PATH + "/{id}", async (string id, DriverProfileDb db) =>
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
