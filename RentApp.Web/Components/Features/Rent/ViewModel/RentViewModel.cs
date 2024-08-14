using RentApp.Web.Components.Core;
using RentApp.Web.Components.Features.Common;
using RentApp.Web.Components.Features.Interfaces;
using RentApp.Web.Components.Features.Rent.Model;

namespace RentApp.Web.Components.Features.Rent.ViewModel;

public class RentViewModel(
        IDriverProfileGateway profileGateway,
        IPlanGateway planGateway,
        IRentalGateway rentalGateway
    ) : AuthViewModelBase, IRentViewModel
{
    private IDriverProfileGateway ProfileGateway => profileGateway;
    private IPlanGateway PlanGateway => planGateway;
    private IRentalGateway RentalGateway => rentalGateway;
    private RentModel model = new([], null, DateOnly.FromDateTime(DateTime.Now), RentState.Loading);

    public RentModel Model
    {
        private set
        {
            SetValue(ref model, value);
        }
        get
        {
            return model;
        }
    }

    public PlanModel? SelectedPlan
    {
        get => Model.SelectedPlan;
        set
        {
            Model = Model with { SelectedPlan = value }; 
        }
    }

    public override void OnAuthInitialized()
    {
        base.OnAuthInitialized();
        EnforceDriverProfile enforceDriverProfile = new() { Gateway = ProfileGateway };
        enforceDriverProfile.EnsureProfileExists(
            GetUserId(),
            LoadRentalUI,
            NavigateTo
        );
    }

    private void ShowLoading()
    {
        Model = Model with { State = RentState.Loading };
    }

    private void LoadRentalUI()
    {
        Console.WriteLine("LoadRentalApi");
        RentalGateway.HasActiveRental(GetUserId(), (rentalOnGoing) =>
        {
            Console.WriteLine("RentalOngoing: " + rentalOnGoing);
            PlanGateway.GetAvailablePlans(
            onResult: (plans) =>
            {
                Model = Model with { AvailablePlans = plans };
                if (rentalOnGoing)
                    Model = Model with { State = RentState.RentalActive };
                else
                    Model = Model with { State = RentState.ShowForm };
            },
            onError: (s) => Console.WriteLine("Error: " + s));
        }, (s) =>
        {
            Console.WriteLine("Error: " + s);
        });
    }

    public void StartNewRental()
    {
        if (Model.SelectedPlan is null)
        {
            Notify("É necessário escolher um plano");
            return;
        }
        ShowLoading();
        RentalGateway.StartNewRental(
            userId: GetUserId(),
            planModel: Model.SelectedPlan,
            onSuccess: LoadRentalUI,
            onError: (s) => Console.WriteLine());
    }
}