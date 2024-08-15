using System.Net;
using RentApp.FrontDataModelLib;

namespace RentApp.Web.Components.Data.Source;


public class MotorcycleRemoteDataSource(HttpClient httpClient)
{
    private const string ENDPOINT = "/motorcycles";

    public async void RegisterMotorcycleAsync(
        Motorcycle motorcycle,
        Action onSuccess,
        Action<string>? onError = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await httpClient.PostAsJsonAsync(ENDPOINT, motorcycle, cancellationToken);
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

    public async void GetMotorcyclesAsync(
        Action<List<Motorcycle>?> onResult,
        Action<string>? onError = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await httpClient.GetFromJsonAsync<List<Motorcycle>>(ENDPOINT,
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

    public async void PutMotorcycle(
        string oldId,
        string newId,
        Action onSuccess,
        Action<string>? onError = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await httpClient.PutAsync(ENDPOINT + $"/{oldId}/{newId}", null, cancellationToken);
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

    public async void DeleteMotorcycleAsync(
        string id,
        Action onResult,
        Action<string>? onError = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await httpClient.DeleteAsync(ENDPOINT + $"/{id}",
                cancellationToken);
            if (response.IsSuccessStatusCode)
            {
                onResult();
            }
            else
            {
                Console.WriteLine("Não é possível remover, a moto deve estar alugada");
                onError?.Invoke("Não é possível remover, a moto deve estar alugada");
            }
        }
        catch (Exception)
        {
            onError?.Invoke("Não é possível remover, a moto deve estar alugada");
        }
    }
}