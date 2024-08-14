using RentApp.Web.Components.Features.ListMotorcycles.Model;
using RentApp.Web.Components.Features.RegisterMotorcycle.Model;

namespace RentApp.Web.Components.Features.Interfaces;


public interface IMotorcycleGateway
{
    void RegisterMotorcycle(RegisterMotorcycleModel model, Action onResult, Action<string> onError);
    void UpdateMotorcycle(string oldPlate, string newPlate, Action onResult, Action<string> onError);
    void DeleteMotorcycle(string licensePlate, Action onResult, Action<string> onError);
    void GetMotorcycles(Action<List<MotorcycleItemModel>> onResult, Action<string> onError);
}