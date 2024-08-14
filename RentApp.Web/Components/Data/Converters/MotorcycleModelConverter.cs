
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
            Year = model.Year is DateOnly dateOnly ? dateOnly.Year : -1,
            LicensePlate = model.LicensePlate,
        };
    }

    public static Motorcycle ToMotorcycle(MotorcycleItemModel model)
    {
        return new Motorcycle
        {
            Identifier = model.Identifier,
            ModelName = model.ModelName,
            Year = model.Year is DateOnly dateOnly ? dateOnly.Year : -1,
            LicensePlate = model.LicensePlate,
        };
    }

    public static MotorcycleItemModel ToMotorcycleItem(Motorcycle model)
    {
        return new MotorcycleItemModel
        {
            Identifier = model.Identifier,
            ModelName = model.ModelName,
            Year = model.Year > 0 ? new DateOnly(model.Year, 1, 1) : null,
            LicensePlate = model.LicensePlate,
        };
    }
}