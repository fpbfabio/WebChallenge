using System.Net;
using RentApp.BackDataModelLib;

namespace RentApp.ApiService.Clients;

public class RentalApiClient(HttpClient httpClient)
{
    private const string API_ENDPOINT = "/rentalapi";

    public async Task StartNewRental(
        string userId,
        int planId,
        Action onSuccess,
        Action<string> onError,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await httpClient.PostAsync($"{API_ENDPOINT}/{userId}/{planId}", null, cancellationToken);
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

    public async Task GetUserRentals(
        string userId,
        Action<List<RentalApiDataModel>?> onResult,
        Action<string> onError,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await httpClient.GetFromJsonAsync<List<RentalApiDataModel>>($"{API_ENDPOINT}/{userId}",
                cancellationToken);
            await Task.Run(() => onResult(response), cancellationToken);
        }
        catch (HttpRequestException exception)
        {
            if (exception.StatusCode == HttpStatusCode.NotFound)
            {
                onResult(null);
                return;
            }
            onError?.Invoke(exception.ToString());
        }
    }
}