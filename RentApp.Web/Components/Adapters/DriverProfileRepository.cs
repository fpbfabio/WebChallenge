using RentApp.Web.Components.Data.DriverProfile.DataSource;
using RentApp.Web.Components.Data.DriverProfile.Model;
using RentApp.Web.Components.Features.Interfaces;
using RentApp.Web.Components.Features.RegisterProfile.Model;

namespace RentApp.Web.Components.Adapters;


public class DriverProfileRepository(DriverProfileRemoteDataSource driverProfileRemoteDataSource) : IDriverProfileGateway
{
    private DriverProfileRemoteDataSource DriverProfileRemoteDataSource => driverProfileRemoteDataSource;

    private static DriverProfileData MapRegisterProfileModelToDriverProfileData(RegisterProfileModel model)
    {
        return new DriverProfileData
        {
            Name = model.Name,
            CompanyCode = model.CompanyCode,
            BirthDate = model.BirthDate.ToLongDateString(),
            Category = model.Category,
            DriverLicenseCode = model.DriverLicenseCode
        };
    }

    public void ProfileExists(string id, Action<bool> onResult, Action<string> onError)
    {
        DriverProfileRemoteDataSource.ProfileExists(id, onResult, onError);
    }

    public void RegisterProfile(string id, RegisterProfileModel model)
    {
        DriverProfileData driverProfileData = MapRegisterProfileModelToDriverProfileData(model);
        driverProfileData.Id = id;
        DriverProfileRemoteDataSource.RegisterDriverProfileAsync(driverProfileData, () => {
            Console.WriteLine("User signed up");
        });
    }
}