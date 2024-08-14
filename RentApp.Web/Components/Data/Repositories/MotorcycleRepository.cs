using RentApp.Web.Components.Features.Interfaces;
using RentApp.FrontDataModelLib;
using RentApp.Web.Components.Data.Source;
using RentApp.Web.Components.Features.RegisterMotorcycle.Model;
using RentApp.Web.Components.Data.Converters;
using RentApp.Web.Components.Features.ListMotorcycles.Model;

namespace RentApp.Web.Components.Data.Repositories;


public class MotorcycleRepository(MotorcycleRemoteDataSource motorcycleRemoteDataSource) : IMotorcycleGateway
{
    private MotorcycleRemoteDataSource MotorcycleRemoteDataSource => motorcycleRemoteDataSource;

    public void RegisterMotorcycle(RegisterMotorcycleModel model, Action onResult, Action<string> onError)
    {
        Motorcycle motorcycle = MotorcycleModelConverter.ToMotorcycle(model);
        MotorcycleRemoteDataSource.RegisterMotorcycleAsync(motorcycle, onResult, onError);
    }

    public void UpdateMotorcycle(string oldPlate, string newPlate, Action onResult, Action<string> onError)
    {
        MotorcycleRemoteDataSource.PutMotorcycle(oldPlate, newPlate, onResult, onError);
    }

    public void DeleteMotorcycle(string licensePlate, Action onResult, Action<string> onError)
    {
        MotorcycleRemoteDataSource.DeleteMotorcycleAsync(licensePlate, onResult, onError);
    }

    public void GetMotorcycles(Action<List<MotorcycleItemModel>> onResult, Action<string> onError)
    {
        MotorcycleRemoteDataSource.GetMotorcyclesAsync((motorcycles) =>
        {
            if (motorcycles is null)
            {
                onResult([]);
                return;
            }
            var motorcycleItems = from x in motorcycles select MotorcycleModelConverter.ToMotorcycleItem(x);
            onResult(motorcycleItems.ToList());
        }, onError);
    }
}