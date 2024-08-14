using RentApp.ApiService.Clients;
using RentApp.ApiService.Converters;
using RentApp.FrontDataModelLib;

namespace RentApp.ApiService.Server;

public static class PlanEndpoints
{
    private const string ENDPOINT = "/plans";
    private const string DEFAULT_ERROR_DETAIL = "Internal server error";

    public static void RegisterPlanEndpoints(this WebApplication app)
    {
        app.MapGet(ENDPOINT + "/{id}", async (int id) =>
        {
            PlanApiClient? client = app.Services.GetService<PlanApiClient>();
            IResult result = TypedResults.Problem(detail: DEFAULT_ERROR_DETAIL);
            if (client is null)
            {
                return result;
            }
            await client.GetPlan(id, (planApiDataModel) =>
            {
                if (planApiDataModel is null)
                {
                    result = TypedResults.Problem(detail: $"No plan with id: {id}");
                    return;
                }
                Plan plan = PlanConverter.ToFrontModel(planApiDataModel);
                result = TypedResults.Ok(plan);
            }, (s) =>
            {
                result = TypedResults.Problem(detail: s);
            });
            return result;
        });
        app.MapGet(ENDPOINT, async () =>
        {
            PlanApiClient? client = app.Services.GetService<PlanApiClient>();
            IResult result = TypedResults.Problem(detail: DEFAULT_ERROR_DETAIL);
            if (client is null)
            {
                return result;
            }
            await client.GetAvailablePlans((planApiDataModelList) =>
            {
                if (planApiDataModelList is null)
                {
                    result = TypedResults.Problem(detail: $"No plans");
                    return;
                }
                var plans = from x in planApiDataModelList select PlanConverter.ToFrontModel(x);
                result = TypedResults.Ok(plans.ToList());
            }, (s) =>
            {
                result = TypedResults.Problem(detail: s);
            });
            return result;
        });
    }
}