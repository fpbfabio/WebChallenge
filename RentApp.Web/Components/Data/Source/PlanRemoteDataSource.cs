using RentApp.FrontDataModelLib;

namespace RentApp.Web.Components.Data.Source;


public class PlanRemoteDataSource(HttpClient httpClient)
{
    private const string API_ENDPOINT = "/plans";

    public async void GetPlan(
        int id,
        Action<Plan?> onSuccess,
        Action<string> onError,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await httpClient.GetFromJsonAsync<Plan>($"{API_ENDPOINT}/{id}",
                cancellationToken);
            onSuccess(response);
        }
        catch (HttpRequestException exception)
        {
            onError?.Invoke(exception.ToString());
        }
    }

    public async void GetAvailablePlans(
        Action<List<Plan>> onSuccess,
        Action<string> onError,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await httpClient.GetFromJsonAsync<List<Plan>>($"{API_ENDPOINT}",
                cancellationToken);
            response ??= [];
            onSuccess(response);
        }
        catch (HttpRequestException exception)
        {
            onError?.Invoke(exception.ToString());
        }

    }
}