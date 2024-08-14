namespace RentApp.Web.Components.Features.Rent.Model;

public record RentalModel
(
    PlanModel SelectedPlan,
    DateOnly StartDate
);