using RentApp.Web.Components.Core;
using RentApp.Web.Components.Features.Interfaces;
using RentApp.Web.Components.Features.RegisterMotorcycle.Model;

namespace RentApp.Web.Components.Features.RegisterMotorcycle.ViewModel;

public class RegisterMotorcycleViewModel(IMotorcycleGateway motorcycleGateway) : AuthViewModelBase, IRegisterMotorcycleViewModel
{
    private IMotorcycleGateway Gateway => motorcycleGateway;

    private RegisterMotorcycleModel model = new();

    private RegisterMotorcycleModel Model
    {
        set
        {
            SetValue(ref model, value);
        }
        get
        {
            return model;
        }
    }

    public string? Identifier 
    {
        get => Model.Identifier;
        set
        {
            Model = Model with { Identifier = value };
        }
    }

    public DateOnly? Year
    {
        get => Model.Year;
        set
        {
            Model = Model with { Year = value };
        }
    }

    public string? ModelName
    {
        get => Model.ModelName;
        set
        {
            Model = Model with { ModelName = value };
        }
    }

    public string? LicensePlate
    {
        get => Model.LicensePlate;
        set
        {
            Model = Model with { LicensePlate = value };
        }
    }

    public void Save()
    {
        Gateway.RegisterMotorcycle(Model, () =>
        {
            Model = new();
            Notify("Motorcycle added");
        }, (s) =>
        {
            Console.WriteLine("Error at RegisterMotorcycleViewModel.Save: " + s);
        });
    }
}