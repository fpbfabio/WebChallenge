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
    private RentModel model = new([]);

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

    public DateOnly? EndDate
    {
        get => Model.EndDate;
        set
        {
            Model = Model with { EndDate = value };
            UpdatePrice();
        }
    }

    public string? Price
    {
        get => Model.Price;
        set
        {
            Model = Model with { Price = value };
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
        RentalGateway.GetActiveUserRental(GetUserId(), (activeRental) =>
        {
            PlanGateway.GetAvailablePlans(
            onResult: (plans) =>
            {
                Model = Model with { AvailablePlans = plans };
                if (activeRental != null)
                    Model = Model with
                    {
                        State = RentState.RentalActive,
                        ActiveRentalId = activeRental.Id
                    };
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
            onError: (s) => {
                LoadRentalUI();
                Notify(s);
            });
    }

    private void UpdatePrice()
    {
        Price = null;
        if (Model.EndDate is null || Model.ActiveRentalId is null)
        {
            LoadRentalUI();
            return;
        }
        RentalGateway.GetPriceForDate(Model.ActiveRentalId, (DateOnly) Model.EndDate, (p) =>
        {
            if (p < 0)
            {
                Notify($"Data de fim precisa ser maior que a de inicio da locação");
                Price = null;
            }
            else
            {
                Price = $"R$ {p}";
            }
        }, (s) =>
        {
            Console.WriteLine("RentViewModel: Error on UpdatePrice(): " + s);
        });
    }

    public void EndRental()
    {
        if (Model.Price is null)
        {
            Notify($"Selecione uma data de fim para a locação");
            return;
        }
        if (Model.EndDate is null || Model.ActiveRentalId is null)
        {
            LoadRentalUI();
            return;
        }
        ShowLoading();
        RentalGateway.EndRental(Model.ActiveRentalId, (DateOnly) Model.EndDate, () =>
        {
            LoadRentalUI();
        }, (s) =>
        {
            Console.WriteLine("RentViewModel: Error on EndRental(): " + s);
        });
    }
}