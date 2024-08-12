using System.ComponentModel;
using RentApp.Web.Components.Features.SignUp.Model;

namespace RentApp.Web.Components.Features.SignUp.ViewModel;

public interface ISignUpViewModel : INotifyPropertyChanged
{
    public string Name { set; get; }

    public string CompanyCode { set; get; }

    public DateOnly BirthDate { set; get; }

    public string DriverLicenseCode { set; get; }

    public string Category { set; get; }

    public List<string> Options { get; }
    public void Save();
}