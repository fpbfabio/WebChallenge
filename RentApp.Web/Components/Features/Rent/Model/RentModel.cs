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
    PlanModel? SelectedPlan = null,
    RentState State = RentState.Loading,
    string? Price = null,
    DateOnly? EndDate = null,
    string? ActiveRentalId = null
);