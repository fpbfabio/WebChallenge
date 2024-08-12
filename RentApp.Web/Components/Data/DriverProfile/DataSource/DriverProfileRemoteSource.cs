using RentApp.Web.Components.Data.DriverProfile.Model;

namespace RentApp.Web.Components.Data.DriverProfile.DataSource;


public class DriverProfileRemoteDataSource(HttpClient httpClient)
{
    public async void ProfileExists(
        string id,
        Action<bool> onSuccess,
        Action<string>? onError = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await httpClient.GetAsync($"/driverprofiles/{id}",
                HttpCompletionOption.ResponseContentRead,
                cancellationToken);
            onSuccess(response.IsSuccessStatusCode);
        }
        catch (HttpRequestException exception)
        {
            onError?.Invoke(exception.ToString());
        }
    }

    public async void RegisterDriverProfileAsync(
        DriverProfileData driverProfile,
        Action onSuccess,
        Action<string>? onError = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await httpClient.PostAsJsonAsync("/driverprofiles", driverProfile, cancellationToken);
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