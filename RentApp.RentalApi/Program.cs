using Microsoft.EntityFrameworkCore;
using RentApp.BackDataModelLib;
using RentApp.RentalApi.Contexts;
using RentApp.RentalApi.Converters;
using RentApp.RentalApi.Models;

const string ENDPOINT = "/rentalapi";

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire components.
builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddProblemDetails();
string? connectionString = builder.Configuration.GetConnectionString("mongodb");
if (connectionString != null)
{
    builder.Services.AddDbContext<RentalDb>(opt =>
        opt.UseMongoDB(connectionString, "rent_app"));
}
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseExceptionHandler();

app.MapGet(ENDPOINT, async (RentalDb db) =>
    await db.Items.ToListAsync());

app.MapGet(ENDPOINT + "/rental/{rentalId}", async (string rentalId, RentalDb db) =>
    await Task.Run(async () => {
        var rental = await db.FindAsync<RentalDatabaseModel>(rentalId);
        if (rental is null)
        {
            return Results.NotFound();
        }
        return Results.Ok(ModelConverter.ToApiDataModel(rental));
    }));

app.MapGet(ENDPOINT + "/user/{userId}", async (string userId, RentalDb db) =>
    await Task.Run(() => {
        var userRentals = db.Items.Where(x => x.UserId == userId).ToList();
        if (userRentals is null || userRentals.Count == 0)
        {
            return Results.NotFound();
        }
        var list = (from x in userRentals select ModelConverter.ToApiDataModel(x)).ToList();
        return Results.Ok(userRentals);
    }));

app.MapPost(ENDPOINT + "/start/{userId}/{planId}", async (string userid, int planId, RentalDb db) =>
{
    RentalApiDataModel rental = new ()
    {
        PlanId = planId,
        StartDate = DateOnly.FromDateTime(DateTime.Today).DayNumber,
    };
    RentalDatabaseModel databaseModel = ModelConverter.ToDatabaseModel(rental);
    databaseModel.UserId = userid;
    db.Items.Add(databaseModel);
    await db.SaveChangesAsync();
    return Results.Created($"/rentals/{userid}/{planId}", rental);
});

app.MapPost(ENDPOINT + "/end/{rentalId}/{date}", async (string rentalId, int date, RentalDb db) =>
{
    RentalDatabaseModel? databaseModel = await db.Items.FindAsync(rentalId);
    if (databaseModel != null)
    {
        databaseModel.EndDate = date;
    }
    await db.SaveChangesAsync();
    return Results.Ok();
});

app.Run();
