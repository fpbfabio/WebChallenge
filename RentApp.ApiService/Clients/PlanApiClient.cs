using RentApp.BackDataModelLib;

namespace RentApp.ApiService.Clients;


public class PlanApiClient(HttpClient httpClient)
{
    private const string API_ENDPOINT = "/planapi";

    public async Task GetPlan(
        int id,
        Action<PlanApiDataModel?> onSuccess,
        Action<string> onError,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await httpClient.GetFromJsonAsync<PlanApiDataModel>($"{API_ENDPOINT}/{id}",
                cancellationToken);
            onSuccess(response);
        }
        catch (HttpRequestException exception)
        {
            onError?.Invoke(exception.ToString());
        }
    }

    public async Task GetAvailablePlans(
        Action<List<PlanApiDataModel>> onSuccess,
        Action<string> onError,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await httpClient.GetFromJsonAsync<List<PlanApiDataModel>>($"{API_ENDPOINT}",
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