using RentApp.ApiService.Clients;
using RentApp.ApiService.Converters;
using RentApp.BackDataModelLib;
using RentApp.FrontDataModelLib;

namespace RentApp.ApiService.Server;

public static class MotorcycleEndpoints
{
    private const string ENDPOINT = "/motorcycles";
    private const string DEFAULT_ERROR_DETAIL = "Internal server error";

    public static void RegisterMotorcycleEndpoints(this WebApplication app)
    {
        RegisterPutMotorcycle(app);
        RegisterPostMotorcycle(app);
        RegisterGetMotorcycles(app);
        RegisterDeleteMotorcycle(app);
    }

    private static void RegisterPostMotorcycle(WebApplication app)
    {
        app.MapPost(ENDPOINT, async (Motorcycle motorcycle) =>
        {
            MotorcycleApiClient? client = app.Services.GetService<MotorcycleApiClient>();
            IResult result = TypedResults.Problem(detail: DEFAULT_ERROR_DETAIL);
            if (client != null)
            {
                MotorcycleApiDataModel motorcycleApiDataModel = MotorcycleConverter.ToApiDataModel(motorcycle);
                await client.PostMotorcycleAsync(motorcycleApiDataModel, () =>
                {
                    result = TypedResults.Created($"{ENDPOINT}/{motorcycle.LicensePlate}", motorcycle);
                }, (s) =>
                {
                    result = TypedResults.Problem(detail: s);
                });
            }
            return result;
        });
    }

    private static void RegisterGetMotorcycles(WebApplication app)
    {
        app.MapGet(ENDPOINT, async () =>
        {
            MotorcycleApiClient? client = app.Services.GetService<MotorcycleApiClient>();
            IResult result = TypedResults.Problem(detail: DEFAULT_ERROR_DETAIL);
            if (client is null)
            {
                Console.WriteLine("MotorcycleEndpoints: MotorcycleApiClient is null");
                return result;
            }
            await client.GetMotorcycles((motorcyclesApiDataModels) =>
            {
                if (motorcyclesApiDataModels is null)
                {
                    Console.WriteLine("MotorcycleEndpoints: NotFound");
                    result = TypedResults.NotFound();
                    return;
                }
                var motorcycles = from x in motorcyclesApiDataModels select MotorcycleConverter.ToFrontModel(x);
                result = TypedResults.Ok(motorcycles.ToList());
            }, (s) =>
            {
                Console.WriteLine("MotorcycleEndpoints: GetMotorcycles failure");
                result = TypedResults.Problem(detail: s);
            });
            return result;
        });
    }

    private static void RegisterPutMotorcycle(WebApplication app)
    {
        app.MapPut(ENDPOINT + "/{oldId}/{newId}", async (string oldId, string newId) =>
        {
            MotorcycleApiClient? client = app.Services.GetService<MotorcycleApiClient>();
            IResult result = TypedResults.Problem(detail: DEFAULT_ERROR_DETAIL);
            if (client is null)
            {
                return result;
            }
            MotorcycleApiDataModel? motorcycleApiDataModel = null;
            await client.GetMotorcycle(oldId, (dataModel) =>
            {
                motorcycleApiDataModel = dataModel;
            }, (s) =>
            {
                result = TypedResults.Problem(detail: s);
            });
            if (motorcycleApiDataModel is null)
            {
                return result;
            }
            motorcycleApiDataModel.LicensePlate = newId;
            bool newMotorcycleAdded = false;
            await client.PostMotorcycleAsync(motorcycleApiDataModel, () =>
            {
                newMotorcycleAdded = true;
            }, (s) =>
            {
                result = TypedResults.Problem(detail: s);
            });
            if (!newMotorcycleAdded)
            {
                return result;
            }
            motorcycleApiDataModel.LicensePlate = oldId;
            motorcycleApiDataModel.ActiveUserId = null;
            bool rentalCleared = false;
            await client.PutMotorcycle(motorcycleApiDataModel, () =>
            {
                rentalCleared = true;
            }, (s) =>
            {
                result = TypedResults.Problem(detail: s);
            });
            if (!rentalCleared)
            {
                return result;
            }
            await client.DeleteMotorcycle(oldId, () =>
            {
                result = TypedResults.Ok();
            }, (s) =>
            {
                result = TypedResults.Problem(detail: s);
            });
            return result;
        });
    }

    private static void RegisterDeleteMotorcycle(WebApplication app)
    {
        app.MapDelete(ENDPOINT + "/{id}", async (string id) =>
        {
            MotorcycleApiClient? client = app.Services.GetService<MotorcycleApiClient>();
            IResult result = TypedResults.Problem(detail: DEFAULT_ERROR_DETAIL);
            if (client != null)
            {
                await client.DeleteMotorcycle(id, () =>
                {
                    result = TypedResults.Ok();
                }, (s) =>
                {
                    result = TypedResults.BadRequest(s);
                });
            }
            return result;
        });
    }
}