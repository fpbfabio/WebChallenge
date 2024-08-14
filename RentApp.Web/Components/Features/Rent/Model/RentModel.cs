namespace RentApp.Web.Components.Features.Rent.Model;

public enum RentState
{
    Loading,
    ShowForm,
    RentalActive
}

public record RentModel
(
    List<PlanModel> AvailablePlans,
    PlanModel? SelectedPlan,
    DateOnly ExpectedEndDate,
    RentState State = RentState.Loading
);