using RentApp.BackDataModelLib;
using RentApp.FrontDataModelLib;

namespace RentApp.ApiService.Converters;

public static class MotorcycleConverter
{
    public static Motorcycle ToFrontModel(MotorcycleApiDataModel motorcycleApiDataModel)
    {
        return new Motorcycle()
        {
            LicensePlate = motorcycleApiDataModel.LicensePlate,
            Year = motorcycleApiDataModel.Year,
            Identifier = motorcycleApiDataModel.Identifier,
            ModelName = motorcycleApiDataModel.ModelName
        };
    }

    internal static MotorcycleApiDataModel ToApiDataModel(Motorcycle motorcycle)
    {
        return new MotorcycleApiDataModel()
        {
            LicensePlate = motorcycle.LicensePlate,
            Year = motorcycle.Year,
            Identifier = motorcycle.Identifier,
            ModelName = motorcycle.ModelName
        };
    }
}