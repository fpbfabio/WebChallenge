namespace RentApp.Web.Components.Features.Rent.Model;

public record RentalModel
(
    string? Id,
    PlanModel SelectedPlan,
    DateOnly StartDate
);