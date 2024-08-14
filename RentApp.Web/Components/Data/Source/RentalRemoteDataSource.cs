using System.Net;
using RentApp.FrontDataModelLib;

namespace RentApp.Web.Components.Data.Source;


public class RentalRemoteDataSource(HttpClient httpClient)
{
    private const string ENDPOINT = "/rentals";
    public async void StartNewRental(
        string userId,
        int planId,
        Action onSuccess,
        Action<string> onError,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await httpClient.PostAsync($"{ENDPOINT}/{userId}/{planId}", null, cancellationToken);
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

    public async void GetActiveUserRental(
        string userId,
        Action<Rental?> onResult,
        Action<string> onError,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await httpClient.GetFromJsonAsync<Rental>($"{ENDPOINT}/{userId}",
                cancellationToken);
            onResult(response);
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