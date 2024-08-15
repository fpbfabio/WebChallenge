using System.Security.Claims;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Identity.Web;

namespace RentApp.Web.Components.Core;

public class AuthRazorBase<T> : RazorBase<T> where T : IAuthViewModelBase
{

    [CascadingParameter]
    public required Task<AuthenticationState> AuthenticationState { protected get; set; }

    public required ClaimsPrincipal AuthenticatedUser { protected get; set; }
    protected string UserId => AuthenticatedUser.Claims.First(
        c => c.Type == ClaimConstants.NameIdentifierId).Value;

    private bool IsUserAuthenticated()
    {
        if (AuthenticatedUser == null)
        {
            return false;
        }
        if (AuthenticatedUser.Identity == null)
        {
            return false;
        }
        return AuthenticatedUser.Identity.IsAuthenticated;
    }

    private bool IsInRole(string role)
    {
        return AuthenticatedUser.IsInRole(role);
    }

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        var state = await AuthenticationState;
        AuthenticatedUser = state.User;
        ViewModel.IsInRole = IsInRole;
        ViewModel.IsUserAuthenticated = IsUserAuthenticated;
        ViewModel.GetUserId = () => UserId;
        ViewModel.OnAuthInitialized();
    }
}