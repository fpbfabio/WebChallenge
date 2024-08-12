using RentApp.Web.Components.Core;
using RentApp.Web.Components.Features.Common;
using RentApp.Web.Components.Features.Interfaces;

namespace RentApp.Web.Components.Features.Rent.ViewModel;

public class RentViewModel(IDriverProfileGateway profileGateway) : AuthViewModelBase, IRentViewModel
{
    private IDriverProfileGateway ProfileGateway => profileGateway;

    public override void OnAuthInitialized()
    {
        base.OnAuthInitialized();
        EnforceDriverProfile enforceDriverProfile = new() { Gateway = ProfileGateway };
        enforceDriverProfile.EnsureProfileExists(
            GetUserId(),
            ShowMotorcycles,
            NavigateTo
        );
    }

    private void ShowMotorcycles()
    {
    }
}