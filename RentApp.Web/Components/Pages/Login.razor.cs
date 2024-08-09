using Microsoft.AspNetCore.Components;
using Radzen;

namespace RentApp.Web.Components.Pages;

public class LoginRazorComponent : ComponentBase
{
    public void OnLogin(LoginArgs args)
    {
    }

    public void OnRegister(NavigationManager navigation)
    {
        navigation.NavigateTo("signup");
    }
}
