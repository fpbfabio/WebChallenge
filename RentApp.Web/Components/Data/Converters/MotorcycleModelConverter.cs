
using RentApp.FrontDataModelLib;
using RentApp.Web.Components.Features.ListMotorcycles.Model;
using RentApp.Web.Components.Features.RegisterMotorcycle.Model;

namespace RentApp.Web.Components.Data.Converters;

public static class MotorcycleModelConverter
{

    public static Motorcycle ToMotorcycle(RegisterMotorcycleModel model)
    {
        return new Motorcycle
        {
            Identifier = model.Identifier,
            ModelName = model.ModelName,
            Year = model.Year is int year ? year : -1,
            LicensePlate = model.LicensePlate,
        };
    }

    public static Motorcycle ToMotorcycle(MotorcycleItemModel model)
    {
        return new Motorcycle
        {
            Identifier = model.Identifier,
            ModelName = model.ModelName,
            Year =  model.Year is int year ? year : -1,
            LicensePlate = model.LicensePlate,
        };
    }

    public static MotorcycleItemModel ToMotorcycleItem(Motorcycle model)
    {
        return new MotorcycleItemModel
        {
            Identifier = model.Identifier,
            ModelName = model.ModelName,
            Year = model.Year > 0 ? model.Year : null,
            LicensePlate = model.LicensePlate,
        };
    }
}