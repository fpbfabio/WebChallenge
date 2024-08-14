using Microsoft.VisualBasic;
using RentApp.BackDataModelLib;

namespace RentApp.ApiService.Rules;

public static class PricingRules
{
    public static float CalculateDefaultCost(PlanApiDataModel planApiDataModel)
    {
        return planApiDataModel.Days * planApiDataModel.Days;
    }

    public static float CalculateCost(DateOnly startDate, DateOnly endDate, PlanApiDataModel planApiDataModel)
    {
        var days = Math.Min(endDate.DayNumber - startDate.DayNumber, 1);
        if (days > planApiDataModel.Days)
        {
            return CalculateDefaultCost(planApiDataModel) + (50 + planApiDataModel.PricePerDay) * (days - planApiDataModel.Days);
        }
        else if (days < planApiDataModel.Days)
        {
            var penalty = planApiDataModel.Days == 7 ? 1.2f : 1.4f;
            return CalculateDefaultCost(planApiDataModel) + (penalty * planApiDataModel.PricePerDay) * (days - planApiDataModel.Days);
        }
        else
        {
            return CalculateDefaultCost(planApiDataModel);
        }
    }
}