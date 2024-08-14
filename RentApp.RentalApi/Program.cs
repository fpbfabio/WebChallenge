using System.Globalization;
using Microsoft.EntityFrameworkCore;
using RentApp.BackDataModelLib;
using RentApp.RentalApi.Contexts;
using RentApp.RentalApi.Converters;
using RentApp.RentalApi.Models;

const string ENDPOINT = "/rentalapi";
const string CONNECTION_STRING = "mongodb://127.0.0.1:27017/?directConnection=true&serverSelectionTimeoutMS=2000&appName=mongosh+2.2.15";

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire components.
builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddProblemDetails();
builder.Services.AddDbContext<RentalDb>(opt =>
    opt.UseMongoDB(CONNECTION_STRING, "rent_app"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseExceptionHandler();

app.MapGet(ENDPOINT, async (RentalDb db) =>
    await db.Items.ToListAsync());

app.MapGet(ENDPOINT + "/{userId}", async (string userId, RentalDb db) =>
    await Task.Run(() => {
        var userRentals = db.Items.Where(x => x.UserId == userId).ToList();
        if (userRentals is null || userRentals.Count == 0)
        {
            return Results.NotFound();
        }
        var list = (from x in userRentals select ModelConverter.ToApiDataModel(x)).ToList();
        return Results.Ok(userRentals);
    }));

app.MapPost(ENDPOINT + "/{userid}/{planId}", async (string userid, int planId, RentalDb db) =>
{
    RentalApiDataModel rental = new ()
    {
        PlanId = planId,
        StartDate = DateOnly.FromDateTime(DateTime.Today).ToString("yyyy.MM.dd", CultureInfo.InvariantCulture),
    };
    RentalDatabaseModel databaseModel = ModelConverter.ToDatabaseModel(rental);
    databaseModel.UserId = userid;
    db.Items.Add(databaseModel);
    await db.SaveChangesAsync();
    return Results.Created($"/rentals/{userid}/{planId}", rental);
});

app.Run();
