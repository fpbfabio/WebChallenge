using RentApp.Web.Components.Core;

namespace RentApp.Web.Components.Features.RegisterMotorcycle.ViewModel;

public interface IRegisterMotorcycleViewModel : IAuthViewModelBase
{
    public string? Identifier { set; get; }

    public int? Year { set; get; }

    public string? ModelName { set; get; }

    public string? LicensePlate { set; get; }

    public void Save();
}