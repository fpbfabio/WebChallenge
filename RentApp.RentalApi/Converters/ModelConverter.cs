using RentApp.BackDataModelLib;
using RentApp.RentalApi.Models;

namespace RentApp.RentalApi.Converters;

public static class ModelConverter
{
    public static RentalApiDataModel ToApiDataModel(RentalDatabaseModel databaseModel)
    {
        return new RentalApiDataModel {
            Id = databaseModel.Id,
            PlanId = databaseModel.PlanId,
            StartDate = databaseModel.StartDate,
            EndDate = databaseModel.EndDate,
            UserId = databaseModel.UserId
        };
    }

    public static RentalDatabaseModel ToDatabaseModel(RentalApiDataModel dataModel)
    {
        return new RentalDatabaseModel {
            PlanId = dataModel.PlanId,
            StartDate = dataModel.StartDate,
            EndDate = dataModel.EndDate,
            UserId = dataModel.UserId
        };
    }

}