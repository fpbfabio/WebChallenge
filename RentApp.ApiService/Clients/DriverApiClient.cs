using RentApp.FrontDataModelLib;

namespace RentApp.ApiService.Clients;

public class DriverApiClient(HttpClient httpClient)
{
    private const string API_ENDPOINT = "/driverapi";

    public async Task ProfileExists(
        string id,
        Action<bool> onSuccess,
        Action<string>? onError = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await httpClient.GetAsync($"{API_ENDPOINT}/{id}",
                HttpCompletionOption.ResponseContentRead,
                cancellationToken);
            onSuccess(response.IsSuccessStatusCode);
        }
        catch (HttpRequestException exception)
        {
            onError?.Invoke(exception.ToString());
        }
    }

    public async Task RegisterDriverProfileAsync(
        DriverProfile driverProfile,
        Action onSuccess,
        Action<string>? onError = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await httpClient.PostAsJsonAsync($"{API_ENDPOINT}", driverProfile, cancellationToken);
            if (response.IsSuccessStatusCode)
            {
                onSuccess();
            }
            else if (response.ReasonPhrase != null && onError != null)
            {
                onError(response.ReasonPhrase);
            }
        }
        catch (HttpRequestException exception)
        {
            onError?.Invoke(exception.ToString());
        }
    }
}