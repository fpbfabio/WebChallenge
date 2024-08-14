using RentApp.BackDataModelLib;
using RentApp.MotorcycleApi.Models;

namespace RentApp.MotorcycleApi.Converter;

public static class ModelConverter
{
    public static MotorcycleApiDataModel ToApiDataModel(MotorcycleDatabaseModel databaseModel)
    {
        return new MotorcycleApiDataModel
        {
            LicensePlate = databaseModel.LicensePlate,
            Identifier = databaseModel.Identifier,
            Year = databaseModel.Year,
            ModelName = databaseModel.ModelName,
        };
    }

    public static MotorcycleDatabaseModel ToDatabaseModel(MotorcycleApiDataModel apiModel)
    {
        return new MotorcycleDatabaseModel
        {
            LicensePlate = apiModel.LicensePlate,
            Identifier = apiModel.Identifier,
            Year = apiModel.Year,
            ModelName = apiModel.ModelName,
        };
    }

}