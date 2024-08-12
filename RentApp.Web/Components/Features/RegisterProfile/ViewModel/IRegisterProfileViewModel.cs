using RentApp.Web.Components.Core;

namespace RentApp.Web.Components.Features.RegisterProfile.ViewModel;

public interface IRegisterProfileViewModel : IAuthViewModelBase
{
    public string Name { set; get; }

    public string CompanyCode { set; get; }

    public DateOnly BirthDate { set; get; }

    public string DriverLicenseCode { set; get; }

    public string Category { set; get; }

    public List<string> Options { get; }
    public void Save();
}