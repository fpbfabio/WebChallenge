using RentApp.Web.Components.Core;
using RentApp.Web.Components.Features.ListMotorcycles.Model;

namespace RentApp.Web.Components.Features.ListMotorcycles.ViewModel;

public interface IListMotorcyclesViewModel : IAuthViewModelBase
{
    ListMotorcyclesModel Model { get; }
    string? EditingLicensePlate { get; set; }

    void Edit(MotorcycleItemModel motorcycleItemModel);
    void Delete(MotorcycleItemModel motorcycleItemModel);
}