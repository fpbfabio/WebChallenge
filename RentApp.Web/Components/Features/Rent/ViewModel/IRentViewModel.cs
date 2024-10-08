using RentApp.Web.Components.Core;
using RentApp.Web.Components.Features.Rent.Model;

namespace RentApp.Web.Components.Features.Rent.ViewModel;

public interface IRentViewModel : IAuthViewModelBase
{
    RentModel Model { get; }
    PlanModel? SelectedPlan { set; get; }
    DateOnly? EndDate { set; get; }
    string? Price { get; }
    void StartNewRental();
    void EndRental();
}