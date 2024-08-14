using RentApp.Web.Components.Features.Interfaces;
using RentApp.Web.Components.Features.RegisterProfile.Model;
using RentApp.FrontDataModelLib;
using RentApp.Web.Components.Data.Source;

namespace RentApp.Web.Components.Data.Repositories;


public class DriverProfileRepository(DriverProfileRemoteDataSource driverProfileRemoteDataSource) : IDriverProfileGateway
{
    private DriverProfileRemoteDataSource DriverProfileRemoteDataSource => driverProfileRemoteDataSource;

    private static DriverProfile MapToDriverProfileData(RegisterProfileModel model)
    {
        return new DriverProfile
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

    public void RegisterProfile(string id, RegisterProfileModel model, Action onResult, Action<string> onError)
    {
        DriverProfile driverProfileData = MapToDriverProfileData(model);
        driverProfileData.Id = id;
        DriverProfileRemoteDataSource.RegisterDriverProfileAsync(driverProfileData, onResult, onError);
    }
}