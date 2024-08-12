using RentApp.Web.Components.Data.DriverProfile.Model;

namespace RentApp.Web.Components.Data.DriverProfile.DataSource;


public class DriverProfileRemoteDataSource(HttpClient httpClient)
{
    public async Task RegisterDriverProfileAsync(DriverProfileData driverProfile, CancellationToken cancellationToken = default)
    {
        await httpClient.PostAsJsonAsync("/driverprofiles", driverProfile, cancellationToken);
    }
}