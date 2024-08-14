using RentApp.FrontDataModelLib;
using RentApp.Web.Components.Features.Rent.Model;

namespace RentApp.Web.Components.Data.Converters;

public static class PlanModelConverter
{
    public static PlanModel ToPlanModel(Plan plan)
    {
        return new PlanModel
        {
            Id = plan.Id,
            Description = plan.Description
        };
    }
}