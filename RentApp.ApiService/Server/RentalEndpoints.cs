using RentApp.ApiService.Clients;
using RentApp.ApiService.Converters;
using RentApp.ApiService.Rules;
using RentApp.BackDataModelLib;
using RentApp.FrontDataModelLib;

namespace RentApp.ApiService.Server;

public static class RentalEndpoints
{
    private const string ENDPOINT = "/rentals";
    private const string DEFAULT_ERROR_DETAIL = "Internal server error";

    private static void RegisterStartRentalApi(WebApplication app)
    {
        app.MapPost(ENDPOINT + "/start/{userId}/{planId}", async (string userId, int planId) =>
        {
            Console.WriteLine($"Starting rent: {userId} {planId}");
            bool error = false;
            RentalApiClient? client = app.Services.GetService<RentalApiClient>();
            MotorcycleApiClient? motorcycleApiClient = app.Services.GetService<MotorcycleApiClient>();
            DriverApiClient? driverApiClient = app.Services.GetService<DriverApiClient>();
            IResult result = TypedResults.Problem(detail: DEFAULT_ERROR_DETAIL);
            if (client is null || motorcycleApiClient is null || driverApiClient is null)
            {
                Console.WriteLine("RentalEndpoints: null apiclients");
                return result;
            }
            DriverProfile? profile = null;
            await driverApiClient.GetProfile(userId, (driverProfile) =>
            {
                profile = driverProfile;
            }, (s) =>
            {
                result = TypedResults.Problem(detail: s);
                error = true;
            });
            if (error || profile is null)
            {
                return result;
            }
            Console.WriteLine($"User name: {profile.Name}");
            if (profile.Category is null || !profile.Category.Contains('A'))
            {
                result = TypedResults.BadRequest("Usuário não é habilitado para categoria A");
                return result;
            }
            Console.WriteLine($"Looking for motorcycles");
            List<MotorcycleApiDataModel>? motorcycleApiDataModels = null;
            await motorcycleApiClient.GetMotorcycles((motorcycles) =>
            {
                motorcycleApiDataModels = motorcycles;
            }, (s) =>
            {
                result = TypedResults.Problem(detail: s);
                error = true;
            });
            if (error)
            {
                return result;
            }
            if (motorcycleApiDataModels is null
                || motorcycleApiDataModels.Count == 0
                || !motorcycleApiDataModels.Where(x => x.ActiveUserlId is null).Any())
            {
                result = TypedResults.BadRequest("Nenhuma motocicleta disponível");
                Console.WriteLine("No available motorcycles");
                return result;
            }
            MotorcycleApiDataModel motorcycleApiDataModel = motorcycleApiDataModels.First(x => x.ActiveUserlId is null);
            motorcycleApiDataModel.ActiveUserlId = userId;
            await motorcycleApiClient.PutMotorcycle(motorcycleApiDataModel, () =>
            {
                Console.WriteLine($"Motorcycle {motorcycleApiDataModel.LicensePlate} assigned to user {userId}");
            }, (s) =>
            {
                error = true;
                result = TypedResults.Problem(detail: s);
            });
            if (error)
            {
                return result;
            }
            await client.StartNewRental(userId, planId, () =>
            {
                result = TypedResults.Created($"{ENDPOINT}/{userId}/{planId}");
            }, (s) =>
            {
                result = TypedResults.Problem(detail: s);
            });
            return result;
        });
    }

    private static void RegisterEndRentalApi(WebApplication app)
    {
        app.MapPost(ENDPOINT + "/end/{rentalId}/{date}", async (string rentalId, int date) =>
        {
            RentalApiClient? client = app.Services.GetService<RentalApiClient>();
            IResult result = TypedResults.Problem(detail: DEFAULT_ERROR_DETAIL);
            if (client is null)
            {
                Console.WriteLine("RentalEndpoints: RentalApiClient is null");
                return result;
            }
            await client.EndRental(rentalId, date, () =>
            {
                result = TypedResults.Ok();
            }, (s) =>
            {
                result = TypedResults.Problem(detail: s);
            });
            return result;
        });
    }

    private static void RegisterGetPriceApi(WebApplication app)
    {
        app.MapGet(ENDPOINT + "/price/{rentalId}/{endDate}", async (string rentalId, int endDate) =>
        {
            RentalApiClient? client = app.Services.GetService<RentalApiClient>();
            IResult result = TypedResults.Problem(detail: DEFAULT_ERROR_DETAIL);
            if (client is null)
            {
                Console.WriteLine("RentalEndpoints: RentalApiClient is null");
                return result;
            }
            RentalApiDataModel? rental = null;
            await client.GetRental(rentalId, (rentalApiDataModel) =>
            {
                rental = rentalApiDataModel;
            }, (s) =>
            {
                Console.WriteLine("RentalEndpoints: RegisterGetPriceApi failure");
                result = TypedResults.Problem(detail: s);
            });
            if (rental is null)
            {
                Console.WriteLine("RentalEndpoints: NotFound");
                result = TypedResults.NotFound();
                return result;
            }
            PlanApiClient? planApiClient = app.Services.GetService<PlanApiClient>();
            if (planApiClient is null)
            {
                Console.WriteLine("RentalEndpoints: PlanApiClient is null");
                return result;
            }
            await planApiClient.GetPlan(rental.PlanId, (planApiDataModel) =>
            {
                if (planApiDataModel is null)
                {
                    result = TypedResults.NotFound();
                    return;
                }
                if (rental.StartDate is null)
                {
                    return;
                }
                var price = PricingRules.CalculateCost(
                    startDate: DateOnly.FromDayNumber((int) rental.StartDate),
                    endDate: DateOnly.FromDayNumber(endDate),
                    planApiDataModel: planApiDataModel
                );
                result = TypedResults.Ok(price);
            }, (s) =>
            {
                Console.WriteLine("RentalEndpoints: RegisterGetPriceApi failure");
                result = TypedResults.Problem(detail: s);
            });
            return result;
        });
    }

    private static void RegisterGetActiveRental(WebApplication app)
    {
        app.MapGet(ENDPOINT + "/active/{userId}", async (string userId) =>
        {
            Console.WriteLine("RentalEndpoints: GetActiveRental");
            RentalApiClient? client = app.Services.GetService<RentalApiClient>();
            IResult result = TypedResults.Problem(detail: DEFAULT_ERROR_DETAIL);
            if (client is null)
            {
                Console.WriteLine("RentalEndpoints: RentalApiClient is null");
                return result;
            }
            RentalApiDataModel? activeRental = null;
            await client.GetUserRentals(userId, (rentalApiDataModels) =>
            {
                if (rentalApiDataModels is null)
                {
                    Console.WriteLine("RentalEndpoints: NotFound");
                    result = TypedResults.NotFound();
                    return;
                }
                activeRental = rentalApiDataModels.FirstOrDefault(x => x.EndDate == null);
            }, (s) =>
            {
                Console.WriteLine("RentalEndpoints: Active rental failure");
                result = TypedResults.Problem(detail: s);
            });
            if (activeRental is null)
            {
                Console.WriteLine("RentalEndpoints: NotFound");
                result = TypedResults.NotFound();
                return result;
            }
            Console.WriteLine("RentalEndpoints: Active rental found");
            PlanApiClient? planApiClient = app.Services.GetService<PlanApiClient>();
            if (planApiClient is null)
            {
                Console.WriteLine("RentalEndpoints: PlanApiClient is null");
                return result;
            }
            await planApiClient.GetPlan(activeRental.PlanId, (planApiDataModel) =>
            {
                if (planApiDataModel is null)
                {
                    result = TypedResults.NotFound();
                    return;
                }
                Plan plan = PlanConverter.ToFrontModel(planApiDataModel);
                Rental rental = new()
                {
                    Id = activeRental.Id,
                    Plan = plan,
                    StartDate = activeRental.StartDate,
                };
                Console.WriteLine("RentalEndpoints: Active rental success");
                result = TypedResults.Ok(rental);
            }, (s) =>
            {
                Console.WriteLine("RentalEndpoints: Active rental failure");
                result = TypedResults.Problem(detail: s);
            });                        

            return result;
        });
    }

    public static void RegisterRentalEndpoints(this WebApplication app)
    {
        RegisterStartRentalApi(app);
        RegisterEndRentalApi(app);
        RegisterGetActiveRental(app);
        RegisterGetPriceApi(app);
    }
}