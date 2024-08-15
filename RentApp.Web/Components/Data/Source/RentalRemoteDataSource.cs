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
            var response = await httpClient.PostAsync($"{ENDPOINT}/start/{userId}/{planId}", null, cancellationToken);
            if (response.IsSuccessStatusCode)
            {
                onSuccess();
            }
            else
            {
                onError?.Invoke("Não há motos disponíveis ou vc não tem a categoria A");
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
            var response = await httpClient.GetFromJsonAsync<Rental>($"{ENDPOINT}/active/{userId}",
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

    public async void GetPriceForDate(
        string rentalId,
        int endDate,
        Action<float> onResult,
        Action<string> onError,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await httpClient.GetFromJsonAsync<float>($"{ENDPOINT}/price/{rentalId}/{endDate}",
                cancellationToken);
            onResult(response);
        }
        catch (HttpRequestException exception)
        {
            onError?.Invoke(exception.ToString());
        }
    }

    public async void EndRental(
        string rentalId,
        int date,
        Action onResult,
        Action<string> onError,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await httpClient.PostAsync($"{ENDPOINT}/end/{rentalId}/{date}", null,
                cancellationToken);
            if (response.IsSuccessStatusCode)
            {
                onResult();
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