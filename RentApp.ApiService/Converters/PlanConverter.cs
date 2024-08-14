using RentApp.BackDataModelLib;
using RentApp.FrontDataModelLib;

namespace RentApp.ApiService.Converters;

public static class PlanConverter
{
    public static Plan ToFrontModel(PlanApiDataModel planApiDataModel)
    {
        return new Plan()
        {
            Id = planApiDataModel.Id,
            Description = planApiDataModel.Description,
        };
    }
}