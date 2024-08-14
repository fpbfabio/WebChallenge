using RentApp.Web.Components.Features.RegisterProfile.Model;
using RentApp.Web.Components.Core;
using RentApp.Web.Components.Features.Interfaces;

namespace RentApp.Web.Components.Features.RegisterProfile.ViewModel;

public class RegisterProfileViewModel(IDriverProfileGateway profileGateway) : AuthViewModelBase, IRegisterProfileViewModel
{
    private IDriverProfileGateway DriverProfileGateway => profileGateway;
    private RegisterProfileModel model = new();

    private RegisterProfileModel RegisterProfileModel
    {
        set
        {
            SetValue(ref model, value);
        }
        get
        {
            return model;
        }
    }

    public string Name
    {
        set
        {
            RegisterProfileModel = RegisterProfileModel with { Name = value };
        }
        get
        {
            return RegisterProfileModel.Name;
        }
    }

    public string CompanyCode
    {
        set
        {
            RegisterProfileModel = RegisterProfileModel with { CompanyCode = value };
        }
        get
        {
            return RegisterProfileModel.CompanyCode;
        }
    }

    public DateOnly BirthDate
    {
        set
        {
            RegisterProfileModel = RegisterProfileModel with { BirthDate = value };
        }
        get
        {
            return RegisterProfileModel.BirthDate;
        }
    }

    public string DriverLicenseCode
    {
        set
        {
            RegisterProfileModel = RegisterProfileModel with { DriverLicenseCode = value };
        }
        get
        {
            return RegisterProfileModel.DriverLicenseCode;
        }
    }

    public string Category
    {
        set
        {
            RegisterProfileModel = RegisterProfileModel with { Category = value };
        }
        get
        {
            return RegisterProfileModel.Category;
        }
    }

    public List<string> Options =>
    [
        "A",
        "B",
        "A+B",
    ];

    public void Save()
    {
        DriverProfileGateway.RegisterProfile(GetUserId(), RegisterProfileModel, () =>
        {
            NavigateTo("/");
        }, (s) =>
        {
            Console.WriteLine("Error at RegisterProfileViewModel.Save: " + s);
        });
    }
}