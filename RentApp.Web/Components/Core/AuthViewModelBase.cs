namespace RentApp.Web.Components.Core;


public abstract class AuthViewModelBase : ViewModelBase, IAuthViewModelBase
{
    public required Func<bool> IsUserAuthenticated { get ; set ; }
    public required Func<string> GetUserId { get ; set ; }

    public virtual void OnAuthInitialized() {}
}