using System.Net;
using RentApp.BackDataModelLib;

namespace RentApp.ApiService.Clients;

public class MotorcycleApiClient(HttpClient httpClient)
{
    private const string API_ENDPOINT = "/motorcycleapi";

    public async Task PostMotorcycleAsync(
        MotorcycleApiDataModel motorcycleApiDataModel,
        Action onSuccess,
        Action<string>? onError = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await httpClient.PostAsJsonAsync($"{API_ENDPOINT}", motorcycleApiDataModel, cancellationToken);
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

    public async Task GetMotorcycles(
        Action<List<MotorcycleApiDataModel>?> onResult,
        Action<string> onError,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await httpClient.GetFromJsonAsync<List<MotorcycleApiDataModel>>($"{API_ENDPOINT}",
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

    public async Task GetMotorcycle(
        string id,
        Action<MotorcycleApiDataModel?> onResult,
        Action<string> onError,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await httpClient.GetFromJsonAsync<MotorcycleApiDataModel?>($"{API_ENDPOINT}/{id}",
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

    public async Task PutMotorcycle(
        MotorcycleApiDataModel motorcycleApiDataModel,
        Action onSuccess,
        Action<string>? onError = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await httpClient.PutAsJsonAsync($"{API_ENDPOINT}", motorcycleApiDataModel, cancellationToken);
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


    public async Task DeleteMotorcycle(
        string id,
        Action onSuccess,
        Action<string>? onError = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await httpClient.DeleteAsync($"{API_ENDPOINT}/{id}", cancellationToken);
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