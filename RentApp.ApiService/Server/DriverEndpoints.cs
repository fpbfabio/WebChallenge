using RentApp.ApiService.Clients;
using RentApp.FrontDataModelLib;

namespace RentApp.ApiService.Server;

public static class DriverEndpoints
{
    private const string DRIVER_ENDPOINT = "/driverprofiles";
    private const string DEFAULT_ERROR_DETAIL = "Internal server error";

    public static void RegisterDriverEndpoints(this WebApplication app)
    {
        app.MapPost(DRIVER_ENDPOINT, async (DriverProfile profile) =>
        {
            DriverApiClient? client = app.Services.GetService<DriverApiClient>();
            IResult result = TypedResults.Problem(detail: DEFAULT_ERROR_DETAIL);
            if (client != null)
            {
                await client.RegisterDriverProfileAsync(profile, () =>
                {
                    result = TypedResults.Created($"{DRIVER_ENDPOINT}/{profile.Id}", profile);
                }, (s) =>
                {
                    result = TypedResults.Problem(detail: s);
                });
            }
            return result;
        });

        app.MapGet(DRIVER_ENDPOINT + "/{id}", async (string id) =>
        {
            DriverApiClient? client = app.Services.GetService<DriverApiClient>();
            IResult result = TypedResults.Problem(detail: DEFAULT_ERROR_DETAIL);
            if (client != null)
            {
                await client.ProfileExists(id, (exists) =>
                {
                    result = exists ? TypedResults.Ok() : TypedResults.NotFound();
                }, (s) =>
                {
                    result = TypedResults.Problem(detail: s);
                });
            }
            return result;
        });
    }
}