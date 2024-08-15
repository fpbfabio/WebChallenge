using RentApp.Web.Components.Core;

namespace RentApp.Web.Components.Features.Home;

public class HomeViewModel : AuthViewModelBase, IHomeViewModel
{

    public override void OnAuthInitialized()
    {
        base.OnAuthInitialized();
        if (IsInRole("Admin"))
        {
            NavigateTo("/admin");
        }
        else
        {
            NavigateTo("/rent");
        }
    }
}