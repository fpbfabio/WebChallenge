using RentApp.Web.Components.Features.SignUp.Model;
using RentApp.Web.Components.Core;
using RentApp.Web.Components.Features.SignUp.Gateway;

namespace RentApp.Web.Components.Features.SignUp.ViewModel;

public class SignUpViewModel(ISignUpGateway signUpGateway) : ViewModelBase, ISignUpViewModel
{
    private ISignUpGateway SignUpGateway => signUpGateway;
    private SignUpModel signUpModel = new();

    private SignUpModel SignUpModel
    {
        set
        {
            SetValue(ref signUpModel, value);
        }
        get
        {
            return signUpModel;
        }
    }

    public string Name
    {
        set
        {
            SignUpModel = SignUpModel with { Name = value };
        }
        get
        {
            return SignUpModel.Name;
        }
    }

    public string CompanyCode
    {
        set
        {
            SignUpModel = SignUpModel with { CompanyCode = value };
        }
        get
        {
            return SignUpModel.CompanyCode;
        }
    }

    public DateOnly BirthDate
    {
        set
        {
            SignUpModel = SignUpModel with { BirthDate = value };
        }
        get
        {
            return SignUpModel.BirthDate;
        }
    }

    public string DriverLicenseCode
    {
        set
        {
            SignUpModel = SignUpModel with { DriverLicenseCode = value };
        }
        get
        {
            return SignUpModel.DriverLicenseCode;
        }
    }

    public string Category
    {
        set
        {
            SignUpModel = SignUpModel with { Category = value };
        }
        get
        {
            return SignUpModel.Category;
        }
    }

    public List<string> Options => [
        "A",
        "B",
        "A+B",
    ];

    public void Save()
    {
        Console.WriteLine("Name: " + SignUpModel.Name);
        Console.WriteLine("CNPJ: " + SignUpModel.CompanyCode);
        Console.WriteLine("BirthDate: " + SignUpModel.BirthDate);
        Console.WriteLine("Category: " + SignUpModel.Category);
        Console.WriteLine("DriverLicenseCode: " + SignUpModel.DriverLicenseCode);
        SignUpGateway.RegisterDriver(SignUpModel);
    }
}