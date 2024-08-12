using Microsoft.AspNetCore.Components;
using Radzen;

namespace RentApp.Web.Components.UI.View.Login;

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
