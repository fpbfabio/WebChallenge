using Microsoft.AspNetCore.Components;
using RentApp.Web.Components.Data.DriverProfile.DataSource;
using RentApp.Web.Components.Data.DriverProfile.Model;
using RentApp.Web.Components.Features.SignUp.Gateway;
using RentApp.Web.Components.Features.SignUp.Model;

namespace RentApp.Web.Components.Adapters;


public class DriverProfileRepository(DriverProfileRemoteDataSource driverProfileRemoteDataSource) : ISignUpGateway
{
    private DriverProfileRemoteDataSource DriverProfileRemoteDataSource => driverProfileRemoteDataSource;

    private static DriverProfileData MapSignUpModelToDriverProfileData(SignUpModel signUpModel)
    {
        return new DriverProfileData
        {
            Name = signUpModel.Name,
            CompanyCode = signUpModel.CompanyCode,
            BirthDate = signUpModel.BirthDate.ToLongDateString(),
            Category = signUpModel.Category,
            DriverLicenseCode = signUpModel.DriverLicenseCode
        };
    }

    public async void RegisterDriver(SignUpModel model)
    {
        DriverProfileData driverProfileData = MapSignUpModelToDriverProfileData(model);
        await DriverProfileRemoteDataSource.RegisterDriverProfileAsync(driverProfileData);
    }
}