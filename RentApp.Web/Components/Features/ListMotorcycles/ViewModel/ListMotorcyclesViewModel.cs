using RentApp.Web.Components.Core;
using RentApp.Web.Components.Features.Interfaces;
using RentApp.Web.Components.Features.ListMotorcycles.Model;

namespace RentApp.Web.Components.Features.ListMotorcycles.ViewModel;

public class ListMotorcyclesViewModel(IMotorcycleGateway motorcycleGateway) : AuthViewModelBase, IListMotorcyclesViewModel
{
    private const int REFRESH_INTERVAL = 5000;

    private IMotorcycleGateway Gateway => motorcycleGateway;

    private ListMotorcyclesModel model = new();

    public ListMotorcyclesModel Model
    {
        get => model;
        set
        {
            SetValue(ref model, value);
        }
    }

    public string? EditingLicensePlate
    {
        get => model.EditingLicensePlate;
        set
        {
            Model = Model with { EditingLicensePlate = value };
        }
    }

    public override void OnAuthInitialized()
    {
        base.OnAuthInitialized();
        System.Timers.Timer refreshTimer = new();
        refreshTimer.Elapsed += (s, e) =>
        {
            Gateway.GetMotorcycles(
                onResult: (motorcycleItemModels) =>
                {
                    Model = Model with { MotorcycleItemModels = motorcycleItemModels };
                },
                onError: (s) =>
                {
                    Console.WriteLine("ListMotorcyclesViewModel: " + s);
                }
            );
        };
        refreshTimer.Interval = REFRESH_INTERVAL;
        refreshTimer.Start();
    }

    public void Edit(MotorcycleItemModel motorcycleItemModel)
    {
        if (motorcycleItemModel.LicensePlate is null || EditingLicensePlate is null)
        {
            return;
        }
        Gateway.UpdateMotorcycle(
            motorcycleItemModel.LicensePlate,
            EditingLicensePlate
            , () =>
        {
            Notify("License plate updated");
        }, (s) =>
        {
            Console.WriteLine("Editing motorcycle failed " + s);
        });
    }

    public void Delete(MotorcycleItemModel motorcycleItemModel)
    {
        if (motorcycleItemModel.LicensePlate is null)
        {
            Console.WriteLine("Unexpected null motorcycle plate");
            return;
        }
        Gateway.DeleteMotorcycle(motorcycleItemModel.LicensePlate, () =>
        {
            Notify("Motorcycle deleted!");
        }, (s) =>
        {
            Console.WriteLine("Editing motorcycle failed " + s);
        });
    }
}